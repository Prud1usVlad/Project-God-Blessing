public class BonusBuildingDialogueBox : DialogueBox
{
    public BonusBuilding building;

    public override bool InitDialogue()
    {
        header = building.buildingName;
        body = building.description;

        var inited = base.InitDialogue();
        
        return inited;
    }
}
