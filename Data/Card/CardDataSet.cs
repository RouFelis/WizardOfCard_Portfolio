using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

using Sirenix.OdinInspector;

namespace BETA.Data
{
    // =========================================================================== CardDataSet

    [CreateAssetMenu(menuName = "BETA/Card/DataSet", fileName = "CardDataSet")]
    public sealed class CardDataSet : DataSet
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        // ================================================== Card

        [TableList] [FoldoutGroup("오브젝트 데이터")]
        public CardScriptableData[] Data;

        // ================================================== Component

        [FoldoutGroup("컴포넌트 데이터")]
        public Dictionary<Enums.CardType, Sprite[]> FrameSprite = new Dictionary<Enums.CardType, Sprite[]>();

        [FoldoutGroup("컴포넌트 데이터")]
        public Sprite[][] ArtworkSprite;

        // ================================================== Prefab

        [FoldoutGroup("프리팹 데이터")]
        public CardObject Prefab;
    } 
}
