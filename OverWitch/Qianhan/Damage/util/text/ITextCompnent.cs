using Assets.OverWitch.Qianhan.Damage.util.text;
using Assets.OverWitch.Qianhan.Long;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITextCompnent : Iterable<ITextCompnent>
{
    ITextCompnent setStyle(Style var1);
    Style GetStyle();
    ITextCompnent appendText(string var1);
    ITextCompnent appendSibling(ITextCompnent var1);
    string getUnformattedText();
    string getFormattedText();
    List<ITextCompnent>getSiblings();
    ITextCompnent createCopy();
}
