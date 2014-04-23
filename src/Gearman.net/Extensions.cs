using System;
using System.Linq;

public static class Extensions
{
    /// <summary>
    /// Get the array slice between the two indexes, similar to Python or other languages' array slices.
    /// Index is inclusive for start index, exclusive for end index. (i.e 0-2 is really elements 0,1)
    /// Credit for this goes to http://dotnetperls.com/array-slice
    /// </summary>
    public static T[ ] Slice<T>(this T[ ] source, int start, int end)
    {
        // Handles negative ends
        if (end < 0)
        {
            end = source.Length - start - end - 1;
        }
        int len = end - start;

        // Return new array
        T[ ] res = new T[ len ];
        for (int i = 0; i < len; i++)
        {
            res[ i ] = source[ i + start ];
        }
        return res;
    }
  }
