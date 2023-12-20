using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Singleton;

using DG.Tweening;

using TMPro;

using Sirenix.OdinInspector;

using System;

namespace BETA
{
    // ==================================================================================================== ShopManager

    public class ShopManager : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== Shop

        // ======================================================= GameObject

        [SerializeField, TitleGroup("���� UI ������Ʈ")]
        private GameObject _shopPanel;

        [SerializeField, TitleGroup("���� UI ������Ʈ")]
        private GameObject _shopNPC;

        [SerializeField, TitleGroup("���� UI ������Ʈ")]
        private GameObject _shopSpeech;

        // ======================================================= Text

        [SerializeField, TitleGroup("���� UI �ؽ�Ʈ")]
        private TMP_Text _shopSpeechTMP;

        [SerializeField, TitleGroup("���� UI �ؽ�Ʈ")]
        private TMP_Text[] _cardPriceTMP = new TMP_Text[5];

        [SerializeField, TitleGroup("���� UI �ؽ�Ʈ")]
        private TMP_Text _manaPriceTMP;

        // =========================================================================== Card

        [SerializeField, TitleGroup("ī��")]
        private GameObject _shopCardCollection;

        // =========================================================================== Data

        // ======================================================= Shop

        [SerializeField, TitleGroup("���� ��� ������")]
        private string[] _quotes;

        // ======================================================= Card

        //[SerializeField, TitleGroup("ī�� ������")]
        //private int[] _cardPrices = new int[5]
        //{
        //    80,
        //    80,
        //    80,
        //    80,
        //    80
        //};

        //[SerializeField, TitleGroup("ī�� ������")]
        //private bool[] _hasSold = new bool[5]
        //{
        //    false,
        //    false,
        //    false,
        //    false,
        //    false
        //};

        [SerializeField, TitleGroup("ī�� ������")]
        private ShopManagerData _data;

        [SerializeField, TitleGroup("ī�� ������")]
        private Dictionary<string, CardEventSystems> _shopCardCommands = new Dictionary<string, CardEventSystems>();

        // =========================================================================== EventDispatcher

        [SerializeField, TitleGroup("���Ŵ��� �̺�Ʈ")]
        private ShopManagerEvent _events;

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Start()
        {
            Refresh();
        }

        private void OnEnable()
        {
            _events.OnGameStart.Listener += OnGameStart;

            _events.OnStageStart.Listener += OnStageStart;

            _events.OnShopEnter.Listener += OnShopEnter;

            _events.OnCardBuy.Listener += OnCardBuy;

            //OnShopEnter(true);
        }

        private void OnDisable()
        {
            _events.OnGameStart.Listener += OnGameStart;

            _events.OnStageStart.Listener -= OnStageStart;

            _events.OnShopEnter.Listener -= OnShopEnter;

            _events.OnCardBuy.Listener -= OnCardBuy;
        }

        // =========================================================================== GameEvent

        // ======================================================= Game

        private void OnGameStart()
        {
            _data.Refresh();
        }

        // ======================================================= Stage

        private void OnStageStart()
        {
            SetCard();
        }

        // =========================================================================== Shop

        private void OnShopEnter(bool isEnter)
        {
            _shopNPC.gameObject.SetActive(isEnter);
            _shopSpeech.gameObject.SetActive(isEnter);

            if (isEnter)
            {
                StartCoroutine(Speech());
            }
            else
            {
                _shopNPC.gameObject.SetActive(isEnter);
            }
        }

        private IEnumerator Speech()
        {
            DOTween.Kill(_shopSpeech.transform);

            var originalScale = _shopSpeech.transform.localScale;
            var quote = UnityEngine.Random.Range(0, _quotes.Length - 1);

            _shopSpeech.SetActive(true);
            _shopSpeech.transform.localScale = Vector3.zero;

            _shopSpeech.transform.DOScale(originalScale, 0.5f).SetEase(Ease.OutBack);

            _shopSpeechTMP.text = _quotes[quote];

            yield return new WaitForSeconds(3.5f);

            _shopSpeech.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InCubic);

            yield return new WaitForSeconds(0.5f);

            _shopSpeech.SetActive(false);

            yield return null;
        }

        // =========================================================================== Card

        private void SetCard()
        {
            var dataSet = DataManager.Instance.GetDataSet<CardDataSet>();

            CardManager.Instance.Cards[CardManager.SHOP].Clear();

            for (var i = 0; i < 5; i++)
            {
                var serialID = UnityEngine.Random.Range(0, dataSet.Data.Length);

                CardManager.Instance.Cards.Add(CardManager.SHOP, new Card(null, serialID));

                _data.HasSold[i] = false;
            }

            CardManager.Instance.SetShopCardUI();

            Refresh();
        }

        private void SetManaUpgradePrice()
        {
            if (EntityManager.Instance.StatsContainer == null)
            {
                return;
            }

            var mana = EntityManager.Instance.StatsContainer.Mana.statValue;

            var price = (mana + 1) * 10;
            var messege = mana == 20 ? "��� ����!" : $"���� Ȱ�� \n\n {price} ����!";

            //if (mana == 20)
            //{
            //    messege = "��� ����!";
            //}
            //else
            //{
            //    messege = $"���� Ȱ�� \n\n {price} ����!";
            //}

            _manaPriceTMP.text = messege;
        }

        public void ManaUpgrade()
        {
            if (EntityManager.Instance.StatsContainer == null)
            {
                return;
            }

            var mana = EntityManager.Instance.StatsContainer.Mana.statValue;

            var price = (mana + 1) * 10;
            //var price = 0;

            if (mana == 20 || EntityManager.Instance.Money < price)
            {
                return;
            }

            if (EntityManager.Instance.Money >= price)
            {
                EntityManager.Instance.SetMoney(-1 * price);

                EntityManager.Instance.StatsContainer.Mana.ChangeStatValue(mana + 1);
            }

            SetManaUpgradePrice();
        }

        public void Refresh()
        {
            var shop = CardManager.Instance.CardObjects[CardManager.SHOP];

            for (var i = 0; i < shop.Count; i++)
            {
                var cardObject = shop[i];

                if (!_data.HasSold[i])
                {
                    cardObject.Commands = _shopCardCommands;
                }
                else
                {
                    cardObject.FrameImage.color = new Color(0.25f, 0.25f, 0.25f);
                    cardObject.ArtworkImage.color = new Color(0.25f, 0.25f, 0.25f);

                    cardObject.Commands = _shopCardCommands; // cannnot
                }

                cardObject.transform.SetParent(_shopCardCollection.transform);
                cardObject.transform.SetSiblingIndex(i);

                _cardPriceTMP[i].text = _data.CardPrices[i].ToString();
            }

            SetManaUpgradePrice();
        }

        private void OnCardBuy(CardObject cardObject)
        {
            var index = CardManager.Instance.CardObjects[CardManager.SHOP].IndexOf(cardObject);

            if (EntityManager.Instance.StatsContainer == null || EntityManager.Instance.Money < _data.CardPrices[index] || _data.HasSold[index])
            {
                return;
            }

            EntityManager.Instance.SetMoney(-1 * _data.CardPrices[index]);

            var card = CardManager.Instance.Cards[CardManager.SHOP, index];

            CardManager.Instance.Add(CardManager.OWN, new Card(null, card.SerialID));

            _data.HasSold[index] = true;

            Refresh();
            CardManager.Instance.CardArrange(CardManager.OWN);
        }
    }
}
