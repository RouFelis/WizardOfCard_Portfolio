using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Data/Manager/Shop")]
public class ShopManagerData : SerializedScriptableObject
{
    // ==================================================================================================== Field

    // =========================================================================== Data

    // ======================================================= Card

    [SerializeField, TitleGroup("카드 데이터")]
    private int[] _cardPrices = new int[5]
    {
        80,
        80,
        80,
        80,
        80
    };

    [SerializeField, TitleGroup("카드 데이터")]
    private bool[] _hasSold = new bool[5]
    {
        false,
        false,
        false,
        false,
        false
    };

    // ==================================================================================================== Property

    // =========================================================================== Data

    // ======================================================= Card

    public int[] CardPrices
    {
        get
        {
            return _cardPrices;
        }

        set
        {
            _cardPrices = value;
        }
    }

    public bool[] HasSold
    {
        get
        {
            return _hasSold;
        }

        set
        {
            _hasSold = value;
        }
    }

    public void Refresh()
    {
        _cardPrices = new int[5]
        {
            80,
            80,
            80,
            80,
            80
        };

        _hasSold = new bool[5]
        {
            false,
            false,
            false,
            false,
            false
        };
    }
}
