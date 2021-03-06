using UnityEngine;

public class ProductionState : InputState {

   public Building selectedBuilding;
   public GameObject selectedPrefab;

   public override void HandleInput(string input, params object[] context) {
      switch (input) {
         case "tapProductionSlot":
            SetSelectedPrefab((GameObject) context[0]);
            break;
         case "trainBtn":
            TrainUnit();
            break;
         default:
            base.HandleInput(input, context); 
            break;
      }
   }

   private void SetSelectedPrefab(GameObject prefab) {
      selectedPrefab = prefab;
   }

   private void TrainUnit() {
      if (selectedPrefab) {
         BattleManager.ChangeFunds(selectedPrefab.GetComponent<Unit>().cost * -1);
         uiManager.UpdateFundsDisplay();
         GridManager.AddUnit(selectedPrefab, selectedBuilding.xy);
         TransitionTo(new BaseState());
      }
   }

   public override void Enter() {
      uiManager.ShowProductionUI(GridManager.GetUnitPrefabs());
   }

   public override void Exit() {
      uiManager.HideProductionUI();
   }

   public ProductionState(Building building) {
      selectedBuilding = building;
   }
}