﻿namespace ClassLibrary1
{
    public interface IMovable
    {
        void MoveX(int offset);
        void MoveY(int offset);
        void MoveZ(int offset);
        void Zero();
        string toString();
    }
}