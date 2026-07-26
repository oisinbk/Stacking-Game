// using Blocks;
// using System.Collections.Generic;
// using System.Reflection;
//
// namespace Pooling
// {
//     public class BlockPool : SimplePool<BlockPlacement>
//     {
//         public void DestroyChildren()
//         {
//             // 1. Destroy all the physical GameObjects in the scene
//             for (int i = 0; i < transform.childCount; i++)
//             {
//                 Destroy(transform.GetChild(i).gameObject);
//             }
//
//             // 2. Use Reflection to force-clear the private collections in SimplePool
//             var poolType = typeof(SimplePool<BlockPlacement>);
//
//             // Clear the _available stack
//             var availableField = poolType.GetField("_available", BindingFlags.NonPublic | BindingFlags.Instance);
//             if (availableField != null)
//             {
//                 var availableStack = (Stack<BlockPlacement>)availableField.GetValue(this);
//                 availableStack.Clear();
//             }
//
//             // Clear the _inUse hashset
//             var inUseField = poolType.GetField("_inUse", BindingFlags.NonPublic | BindingFlags.Instance);
//             if (inUseField != null)
//             {
//                 var inUseSet = (HashSet<BlockPlacement>)inUseField.GetValue(this);
//                 inUseSet.Clear();
//             }
//             
//             // 3. Reset 'size' back to 0. 
//             // This is crucial because your SimplePool's IncreasePool logic uses a loop based on the total 'size'.
//             // Resetting it ensures you don't instantiate too many objects on the next Get() call.
//             var sizeField = poolType.GetField("size", BindingFlags.NonPublic | BindingFlags.Instance);
//             if (sizeField != null)
//             {
//                 sizeField.SetValue(this, 0);
//             }
//         }
//     }
// }