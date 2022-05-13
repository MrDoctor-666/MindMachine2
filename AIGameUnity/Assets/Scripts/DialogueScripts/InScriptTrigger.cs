using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InScriptTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] private TextAsset inkFileSet;
    [Header("If it's a cutscene, please specify: ")]
    [SerializeField] private string cutsceneFolderNameSet;

    public TextAsset inkFile { get => inkFileSet; set => inkFileSet = value; }
    public string cutsceneFolderName { get => cutsceneFolderNameSet; set => cutsceneFolderNameSet = value; }

}
