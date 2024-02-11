using DG.Tweening;
using TMPro;
using UnityEngine;

namespace JudgeTextBeautifier
{
    public class scrHitTextMeshPro : ADOBase
    {
        public void Init(HitMargin hitMargin, string font)
        {
            this.hitMargin = hitMargin;
            text = gameObject.AddComponent<TextMeshPro>();
            gameObject.MakeFlexible();
            meshRenderer = text.renderer;
            if (FontManager.TryGetFont(font, out var fontData))
                text.font = fontData.fontTMP;
            else text.font = FontManager.GetFont("Default").fontTMP;
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 50;
            text.font.material.SetFloat("_PerspectiveFilter", 0);
            text.enableVertexGradient = true;
            gameObject.SetActive(false);
            if (RDString.language != SystemLanguage.Korean && text.font.sourceFontFile == RDConstants.data.latinFont)
                text.fontSize *= 1.1f;
            dead = true;
            ColourSchemeHitMargin hitMarginColours = RDConstants.data.hitMarginColours;
            text.text = RDString.Get("HitMargin." + hitMargin.ToString(), null);
            switch (hitMargin)
            {
                case HitMargin.TooEarly:
                    text.color = hitMarginColours.colourTooEarly;
                    break;
                case HitMargin.VeryEarly:
                    text.color = hitMarginColours.colourVeryEarly;
                    break;
                case HitMargin.EarlyPerfect:
                    text.color = hitMarginColours.colourLittleEarly;
                    break;
                case HitMargin.Perfect:
                    text.color = hitMarginColours.colourPerfect;
                    break;
                case HitMargin.LatePerfect:
                    text.color = hitMarginColours.colourLittleLate;
                    break;
                case HitMargin.VeryLate:
                    text.color = hitMarginColours.colourVeryLate;
                    break;
                case HitMargin.TooLate:
                    text.color = hitMarginColours.colourTooLate;
                    break;
                case HitMargin.Multipress:
                    text.color = hitMarginColours.colourMultipress;
                    break;
                case HitMargin.FailMiss:
                    text.color = hitMarginColours.colourFail;
                    break;
                case HitMargin.FailOverload:
                    text.color = hitMarginColours.colourFail;
                    break;
            }
            scrController instance = scrController.instance;
            gameCam = instance.camy.GetComponent<Camera>();
            forceOnScreen = instance.forceHitTextOnScreen;
            minBorderDistance = instance.hitTextMinBorderDistance;
            meshRenderer.sortingOrder = 100;
        }
        public void Show(Vector3 position, float angle = 0f)
        {
            frameShown = Time.frameCount;
            timer = 0f;
            transform.localPosition = position;
            transform.gameObject.SetActive(true);
            dead = false;
            text.DOKill(false);
            text.color = Color.white;
            if (appearDuration >= 0)
            {
                var (delay, fade) = appearDuration.Distribute(0.41666667f, 0.5833334f);
                text.DOFade(0f, fade).SetDelay(delay).SetEase(Ease.OutQuad);
            }
            else text.DOFade(0f, 0.7f).SetDelay(0.5f).SetEase(Ease.OutQuad);
            scrMisc.Rotate2D(transform, scrController.instance.camy.transform.rotation.eulerAngles.z);
            transform.DOKill(false);
            transform.localScale = new Vector3(startingSize, startingSize, 1f);
            transform.DOPunchScale(new Vector3(sizeUp, sizeUp, 1f), duration, vibrato, elasticity);
            if (hitMargin != HitMargin.Perfect)
                transform.DOLocalRotate(new Vector3(0f, 0f, angle * 20f), 2f, RotateMode.LocalAxisAdd);
            textPos = position;
        }
        public void Update()
        {
            if (dead) return;
            if (forceOnScreen)
            {
                float orthoSize = gameCam.orthographicSize * 2f;
                float factor = orthoSize * Screen.width / Screen.height;
                Vector3 position = gameCam.transform.position;
                Vector3 vector = textPos - position;
                Vector3 localPosition = textPos;
                localPosition.x = position.x + Mathf.Clamp(vector.x, -factor / 2f + minBorderDistance, factor / 2f - minBorderDistance);
                localPosition.y = position.y + Mathf.Clamp(vector.y, -orthoSize / 2f + minBorderDistance, orthoSize / 2f - minBorderDistance);
                transform.localPosition = localPosition;
            }
            timer += Time.deltaTime;
            if (timer > Settings.settings.TextDuration.GraterThan(0, 1.25f))
            {
                dead = true;
                transform.DOKill(false);
                text.DOKill(false);
                gameObject.SetActive(false);
            }
        }
        public TextMeshPro text;
        public bool dead;
		public HitMargin hitMargin;
        public float timer;
        public int frameShown;
        public bool forceOnScreen;
        public float minBorderDistance;
        public Camera gameCam;
        public Vector3 textPos;
        public Renderer meshRenderer;
		public float startingSize = 0.1f;
		public float sizeUp = 0.03f;
		public float duration = 0.15f;
		public int vibrato = 5;
		public float elasticity = 1;

        public float appearDuration = 0.5f;
	}
}
