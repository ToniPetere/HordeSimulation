using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class UnitWaypointGenerator : MonoBehaviour
{
    private NativeArray<float3> GenerateMovePositionArray(float3 _targetPosition, int _positionCount)
    {
        NativeArray<float3> positionArray = new NativeArray<float3>(_positionCount, Allocator.Temp);
        if(_positionCount == 0)
        {
            return positionArray;
        }
        positionArray[0] = _targetPosition;
        if(_positionCount == 1)
        {
            return positionArray;
        }

        float ringSize = 2.2f;
        int ring = 0;
        int positionIndex = 1;

        while(positionIndex  < _positionCount)
        {
            int ringPositionCount = 3 + ring * 2;

            for(int i = 0; i < ringPositionCount; i++)
            {
                float angle = i * (math.PI2 * ringPositionCount); // Calculated Rotation to the next point
                float3 ringVector = math.rotate(quaternion.RotateY(angle), new float3(ringSize * (ring + 1), 0, 0)); // Vector.right + the Rotation
                float3 ringPosition = _targetPosition + ringVector; // Final location in the formation for this Unit

                positionArray[positionIndex] = ringPosition;
                positionIndex++;

                if(positionIndex >= _positionCount)
                {
                    break;
                }
            }
            ring++;
        }

        return positionArray;
    }
}
