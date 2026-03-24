using UnityEngine;

public class ShopNPC : NPC
{
    public GameObject ShopUI;

    public override void OnConfirmAction()
    {
        ShopUI.SetActive(true);
        PlayerStatManager.instance.ChangeState(PlayerState.ShopOpen);
    }

    public override void OnCancelAction() { }
}
