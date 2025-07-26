using UnityEngine;

public interface IMovable
{
    void Move(Vector3 direction);
}

public interface IJumpable
{
    void Jump();
}

public interface IClimbale
{
    void Climb();
}
