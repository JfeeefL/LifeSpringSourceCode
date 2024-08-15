using Core;
using Sirenix.OdinInspector;


[System.Serializable]
public class ContentInfos
{
    public string data;
    [ShowInInspector]
    public Gene thisGene=new Gene();

    public bool isActive=true;
}
