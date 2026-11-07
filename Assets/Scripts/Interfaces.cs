using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interfaces
{
    public interface IInteractable
    {
        void Action();
        bool Grabable { get; set; }
        bool Grabed { get; set; }
    }

    public interface ButtonParent
    {
        void Pressed();
    }
}