using ThinIce.TowerDefence.Plane;

namespace ThinIce.TowerDefence.Input
{
    public class PlateInput : InputControllerBase<TowerPlank>
    {
        public TowerPlank holdingPlate;

        protected override void OnPointerEnter(TowerPlank enterObject)
        {
            enterObject.OnPointerEnter();
            holdingPlate = enterObject;
        }

        protected override void OnPointerExit(TowerPlank exitObject)
        {
            exitObject.OnPointerExit();
            holdingPlate = null;
        }
    }
}