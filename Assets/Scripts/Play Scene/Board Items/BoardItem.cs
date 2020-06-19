using UnityEngine;

namespace BoardItems
{
    public abstract class BoardItem : MonoBehaviour
    {

    }

    public interface ISelectable
    {
        void Select(bool value);
    }

    public interface IMovable
    {
        void MoveStart();
        void MoveStop(bool snap);
    }

    public interface ISnapable
    {
        void Snap();
    }

    public interface IRotable
    {
        void Rotate(Vector3 centre, int angle);
    }

    public interface IBulkRotable
    {
        void BulkRotate(Vector3 centre, int angle);
    }

    public interface IScalable
    {
        void Scale(int value);
    }

    public interface IDuplicable
    {
        GameObject Duplicate();
    }

    public interface IDestroyable
    {
        void Destroy();
    }
}
