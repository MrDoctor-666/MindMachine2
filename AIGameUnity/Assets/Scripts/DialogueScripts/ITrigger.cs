using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrigger
{
    public TextAsset inkFile { get; set; }
    public string cutsceneFolderName { get; set; }
}
