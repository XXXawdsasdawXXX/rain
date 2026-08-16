using Code.Game.Characters.Armor;
using Essential;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters
{
    public abstract class AArmorPieceView<TRender> where TRender : Component
    {
        [field: SerializeField]
        [field: GUIColor(0.4f, 0.8f, 0.5f, 0.9f)]
        public EBodyPart Part { get; private set; }
        [field: SerializeField] public TRender[] Renderers { get; private set; }

        protected AArmorPieceView()
        {
        }

        protected AArmorPieceView(EBodyPart part, TRender[] renderers)
        {
            SetRenderers(part, renderers);
        }

        public void SetRenderers(EBodyPart part, TRender[] renderers)
        {
            Part = part;
            Renderers = renderers;
        }

        public void SetViews(Sprite[] views)
        {
            for (int i = 0; i < Renderers.Length; i++)
            {
                TRender renderer = Renderers[i];

                if (renderer == null) break;

                Sprite sprite = views != null && views.Length > i
                    ? views[i]
                    : null;

                SetSprite(renderer, sprite);
            }
        }

        public Sprite[] GetSprites()
        {
            Sprite[] sprites = new Sprite[Renderers.Length];

            for (int i = 0; i < Renderers.Length; i++)
            {
                TRender renderer = Renderers[i];
                sprites[i] = renderer != null ? GetSprite(renderer) : null;
            }

            return sprites;
        }

        protected abstract void SetSprite(TRender renderer, Sprite sprite);

        protected abstract Sprite GetSprite(TRender renderer);
    }

    public abstract class AArmorRender<TRender, TPieceView>
        where TRender : Component
        where TPieceView : AArmorPieceView<TRender>, new()
    {
        [SerializeReference] protected TPieceView[] render;

        protected CharacterWorldActor actor;
        protected ArmoryLibrary armoryLibrary;

        protected AArmorRender()
        {
            render = CreateDefaultPieces();
        }

        protected virtual TPieceView[] CreateDefaultPieces()
        {
            return new[]
            {
                CreatePiece(EBodyPart.Mask, 1),
                CreatePiece(EBodyPart.Head, 1),
                CreatePiece(EBodyPart.Torso, 1)
            };
        }

        protected static TPieceView CreatePiece(EBodyPart part, int rendererCount)
        {
            TPieceView piece = new();
            piece.SetRenderers(part, new TRender[rendererCount]);
            return piece;
        }

        public void SetArmorLibrary(ArmoryLibrary armoryLibrary)
        {
            this.armoryLibrary = armoryLibrary;
        }

        public void SetHero(CharacterWorldActor actor)
        {
            DisposeHero();

            this.actor = actor;
            this.actor.Model.ArmorChanged += _updateView;

            foreach ((EBodyPart part, int setIndex) in this.actor.Model.Armor) _updateView(part, setIndex);
        }

        public void DisposeHero()
        {
            if (actor == null) return;

            actor.Model.ArmorChanged -= _updateView;
            actor = null;
        }

        public Sprite[] GetCurrentArmorSprites(EBodyPart bodyPart)
        {
            return render[(int)bodyPart].GetSprites();
        }

        public void SetArmorPiece(ArmorPieceModel model)
        {
            int index = (int)model.Part;

            if (index >= render.Length) return;

            render[index].SetViews(model.Views);
        }

        private void _updateView(EBodyPart part, int setIndex)
        {
            if (armoryLibrary.TryGetPiece(setIndex, part, out ArmorPieceModel model)) SetArmorPiece(model);
        }

        [Button]
        public void SetArmorSet(ArmorSetModel armorSetModel)
        {
            if (armorSetModel == null)
            {
                //todo set default view

                Log.Error(this, "[SetArmorSet] set model is null");
                return;
            }

            foreach (ArmorPieceModel armorPieceModel in armorSetModel.Pieces) SetArmorPiece(armorPieceModel);
        }
    }
}