using System;


namespace Tenacity.Behaviour.Enemies.Data
{
    [Flags] public enum ViewDirection { None, Left, Right, Up = 4, Down = 8, Center = 16 }
    public enum State { None, Idle, Moving, Chasing, AttackPreparation, Attacking }
    public enum Type { None, Statue, Standard, Flying }
}