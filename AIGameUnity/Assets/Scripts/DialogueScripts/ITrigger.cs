using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrigger
{
    TextAsset inkFile { get; set; }
    string cutsceneFolderName { get; set; }
}
