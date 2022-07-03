using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

namespace BCPG9 {
    public class ComboCounter : MonoBehaviour {
        [SerializeField] private SpriteAtlas comboNumbers;
        [SerializeField] private GameObject holder;
        [SerializeField] private Image tenImage;
        [SerializeField] private Image oneImage;

        private Dictionary<int, Sprite> spriteSet;
        private WaitForSeconds animWait = new WaitForSeconds(1.333f);

        private void Awake() {
            spriteSet = LoadSprites(comboNumbers);
            holder.SetActive(false);
        }

        public void ShowCombo(int comboCount) {
            if (comboCount < 2)
                return;

            SetComboNumber(comboCount);
            holder.SetActive(true);
            StartCoroutine(ShowComboRoutine());
        }

        IEnumerator ShowComboRoutine() {
            yield return animWait;
            holder.SetActive(false);
        }

        private void SetComboNumber(int comboCount) {
            comboCount = comboCount % 100;
            var ten = comboCount / 10;
            var one = comboCount % 10;

            if (ten < 1) {
                tenImage.gameObject.SetActive(false);
            } else {
                tenImage.gameObject.SetActive(true);
                tenImage.sprite = spriteSet[ten];
            }
            oneImage.sprite = spriteSet[one];
        }

        private static Dictionary<int, Sprite> LoadSprites(SpriteAtlas atlas) {
            var spriteSet = new Dictionary<int, Sprite>();
            var cnSprites = new Sprite[atlas.spriteCount];
            atlas.GetSprites(cnSprites);
            var separator = new char[] { '_', '(' };
            foreach (var sp in cnSprites) {
                spriteSet.Add(int.Parse(sp.name.Split(separator)[1]), sp);
            }
            return spriteSet;
        }
    }
}