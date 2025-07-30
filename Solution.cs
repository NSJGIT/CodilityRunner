using System;
using System.Linq;

namespace CodilityRunner;

class Solution {
    public int solution(int[] H, int X, int Y) {
        // 1) Lines are interchangeable — ensure X ≤ Y for a slightly smaller DP array
        if (X > Y) {
            int t = X; X = Y; Y = t;
        }

        int n = H.Length;
        Array.Sort(H); // Sort ascending, so the first k are the k smallest cars

        // Build a prefix-sum array so we can quickly check total hours of first k cars
        long[] prefix = new long[n + 1];
        for (int i = 1; i <= n; i++) {
            prefix[i] = prefix[i - 1] + H[i - 1];
        }

        // Binary search for the maximum k in [0..n] for which the first k cars can fit
        int lo = 0, hi = n;
        while (lo < hi) {
            int mid = (lo + hi + 1) / 2;
            // If total hours exceed combined capacity, mid is impossible
            if (prefix[mid] > (long)X + Y) {
                hi = mid - 1;
            }
            else {
                // Otherwise, test if we can split those mid cars across two lines
                if (CanPack(H, mid, X, Y, prefix[mid])) {
                    lo = mid;      // feasible → try more cars
                }
                else {
                    hi = mid - 1;  // not feasible → try fewer
                }
            }
        }

        return lo;
    }

    // Check if we can assign the first k cars (H[0..k-1]) into lines of capacity X and Y
    private bool CanPack(int[] H, int k, int X, int Y, long totalHours) {
        // dp[x] = true if some subset of H[0..k-1] sums exactly to x on line X
        var dp = new bool[X + 1];
        dp[0] = true;

        // Standard 1D subset-sum DP (0/1 knapsack) over the k smallest cars
        for (int i = 0; i < k; i++) {
            int h = H[i];
            // iterate backwards so we don't reuse the same car
            for (int x = X; x >= h; x--) {
                if (dp[x - h]) {
                    dp[x] = true;
                }
            }
        }

        // If we put x hours worth of cars on line X,
        // the remainder (totalHours - x) must fit in Y.
        long minX = Math.Max(0, totalHours - Y);
        // scan x from minX up to X for any feasible packing
        for (int x = (int)minX; x <= X; x++) {
            if (dp[x]) return true;
        }
        return false;
    }
}
