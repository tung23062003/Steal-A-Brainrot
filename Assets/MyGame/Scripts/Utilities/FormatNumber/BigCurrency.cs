using System;
using UnityEngine;
/// <summary>
/// 
/// playerCoin = new BigCurrency(999);
/// playerCoin += new BigCurrency(2);
/// 
/// Load PlayerPrefs : playerCoin = PlayerPrefCurrency.LoadCurrency(PlayerPrefKey.Coin, 10000);
/// Save PlayerPrefs : PlayerPrefCurrency.SaveCurrency(PlayerPrefKey.Coin, playerCoin);
/// 
/// 
/// </summary>
[Serializable]
public struct BigCurrency
{
    public double value;
    public int unitIndex; // 0 = "", 1 = K, 2 = M, ...

    private static readonly string[] units = new[]
    {
        "", "K", "M", "B", "T", "P", "E", "aa", "bb"
    };

    public BigCurrency(double amount)
    {
        unitIndex = 0;
        value = amount;

        Normalize();
    }

    public void Normalize()
    {
        while (value >= 1000 && unitIndex < units.Length - 1)
        {
            value /= 1000;
            unitIndex++;
        }

        while (value < 1 && unitIndex > 0)
        {
            value *= 1000;
            unitIndex--;
        }
    }

    public override string ToString()
    {
        if (ToRawValue() < 1000)
            return Mathf.FloorToInt((float)ToRawValue()).ToString();

        return $"{value:0.00}{units[unitIndex]}";
    }


    public double ToRawValue()
    {
        return value * Math.Pow(1000, unitIndex);
    }

    public static BigCurrency FromRaw(double rawValue)
    {
        return new BigCurrency(rawValue);
    }

    public static BigCurrency Clamp(BigCurrency value, BigCurrency min, BigCurrency max)
    {
        if (value < min)
            return min;
        if (value > max)
            return max;
        return value;
    }

    // So sánh
    public static bool operator >=(BigCurrency a, BigCurrency b)
    {
        return a.ToRawValue() >= b.ToRawValue();
    }

    public static bool operator <=(BigCurrency a, BigCurrency b)
    {
        return a.ToRawValue() <= b.ToRawValue();
    }

    public static bool operator >(BigCurrency a, BigCurrency b)
    {
        return a.ToRawValue() > b.ToRawValue();
    }

    public static bool operator <(BigCurrency a, BigCurrency b)
    {
        return a.ToRawValue() < b.ToRawValue();
    }

    public static BigCurrency operator +(BigCurrency a, BigCurrency b)
    {
        return new BigCurrency(a.ToRawValue() + b.ToRawValue());
    }

    public static BigCurrency operator +(BigCurrency a, int b)
    {
        return new BigCurrency(a.ToRawValue() + b);
    }
    public static BigCurrency operator +(float a, BigCurrency b)
    {
        return new BigCurrency(a + b.ToRawValue());
    }

    public static BigCurrency operator -(BigCurrency a, BigCurrency b)
    {
        return new BigCurrency(a.ToRawValue() - b.ToRawValue());
    }

    public static BigCurrency operator -(BigCurrency a, int b)
    {
        return new BigCurrency(a.ToRawValue() - b);
    }

    public static BigCurrency operator -(float a, BigCurrency b)
    {
        return new BigCurrency(a - b.ToRawValue());
    }

    public static BigCurrency operator /(BigCurrency a, BigCurrency b)
    {
        if (b.ToRawValue() == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }
        return new BigCurrency(a.ToRawValue() / b.ToRawValue());
    }

    public static BigCurrency operator /(BigCurrency a, int divisor)
    {
        if (divisor == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }
        return new BigCurrency(a.ToRawValue() / divisor);
    }

    public static BigCurrency operator *(BigCurrency a, double divisor)
    {
        return new BigCurrency(a.ToRawValue() * divisor);
    }

    public static BigCurrency operator *(float a, BigCurrency b)
    {
        return new BigCurrency(a * b.ToRawValue());
    }

    public static BigCurrency operator *(BigCurrency a, BigCurrency b)
    {
        return new BigCurrency(a.ToRawValue() * b.ToRawValue());
    }
}
