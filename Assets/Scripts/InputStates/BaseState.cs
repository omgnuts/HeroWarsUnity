
public class BaseState : InputState {

   public override void HandleInput(string input, params object[] context) {
      switch (input) {
         case "tapUnit":
            //HandleUnitTapped((Unit) context);
            break;
         case "draggingUnit":
            HandleUnitDragStarted((Unit) context[0]);
            break;
         case "tapBuilding":
            HandleBuildingTapped((Building) context[1]); 
            break;
         case "gameMenuBtn":
            TransitionTo(new GameMenuState());
            break;
         default:
            base.HandleInput(input, context); 
            break;
      }
   }

   // private void HandleUnitTapped(Unit unit) {
   //    bool isOwned = unit.owner == BattleManager.GetCurrentPlayerIndex();
   //    if (unit.activated && isOwned) {
   //       TransitionTo(new ActionState(unit));
   //    } else {
   //       TransitionTo(new UnitRangeState(unit));
   //    }
   // }

   private void HandleUnitDragStarted(Unit unit) {
      bool isOwned = unit.owner == BattleManager.GetCurrentPlayerIndex();
      if (unit.activated && isOwned) {
         TransitionTo(new ActionState(unit));
      }
   }

   private void HandleBuildingTapped(Building building) {
      bool isOwned = building.owner == BattleManager.GetCurrentPlayerIndex();
      bool isBarracks = building.type == TileType.barracks;
      if (isOwned && isBarracks) {
         TransitionTo(new ProductionState(building));
      }
   }

   public override void Enter() {
      uiManager.ShowBaseUI();
   }

   public override void Exit() {
      uiManager.HideBaseUI();
   }
}