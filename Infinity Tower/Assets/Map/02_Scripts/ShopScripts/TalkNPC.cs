using UnityEngine;

public class TalkNPC : NPC
{
    public override void OnConfirmAction()
    {
        PlayerStatManager.instance.ChangeState(PlayerState.Interacting);
        NPCUI.instance.SettingUI(NPCName, addDialogueLine, "noting", "noting", null, null);
    }

    public override void OnCancelAction() { }
}
