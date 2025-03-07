using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.Animations;

public class ModifyAnimalAnimations
{
    [MenuItem("HoloTools/Modify Animal Animations")]
    public static void ModifyGroup()
    {
        bool confirm = EditorUtility.DisplayDialog(
            "Modify Animal Animations",
            $"This reconfigures AnimatorController transitions as defined in UsedAssets.cs.\n\n" +
            "Are you sure you want to begin?",
            "Yes", "Cancel"
        );

        if (!confirm)
        {
            Debug.Log("'Modify Animal Animations' canceled.");
            return;
        }

        // Animations are expected to already be in the transition order as they are in the controller
        foreach (var animationGroup in UsedAssets.animations)
        {
            ModifyAnimations(animationGroup);
        }
    }

    static void ModifyAnimations(string[] animationGroup)
    {
        string parentDir = ExtractParentDirectory(animationGroup[0]);
        AnimatorController animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(MakeAnimControllerPath(parentDir));
        var rootStateMachine = animatorController.layers[0].stateMachine;
        var targetState = FindAnimatorState(ExtractAnimationName(animationGroup[0]), rootStateMachine);

        // Same as `Set StateMachine Default State` in the editor
        rootStateMachine.defaultState = targetState;

        foreach (string animation in animationGroup)
        {
            if (!File.Exists(animation))
            {
                Debug.LogError($"Animation not found: {animation}");
                continue;
            }

            string animationName = ExtractAnimationName(animation);
            targetState = FindAnimatorState(animationName, rootStateMachine);

            ChangeAllTransitions(targetState, true);
        }

        // Disable transitions on the final desired state
        ChangeAllTransitions(targetState, false);

        if (animationGroup.Length > 1)
        {
            MakeLoopState(
                FindAnimatorState(ExtractAnimationName(animationGroup[0]), rootStateMachine),
                FindAnimatorState(ExtractAnimationName(animationGroup[^1]), rootStateMachine)
            );
        }

        // Save the modified Animator Controller
        EditorUtility.SetDirty(animatorController);
        AssetDatabase.SaveAssets();

        Debug.Log($"Modified animations for '{parentDir}'.");
    }

    static void MakeLoopState(AnimatorState firstState, AnimatorState lastState)
    {
        string transitionName = "holozoo-loop";
        AnimatorStateTransition transition = null;

        foreach (var existingTransition in lastState.transitions)
        {
            if (existingTransition.name == transitionName)
            {
                transition = existingTransition;
                Debug.Log($"Transition {existingTransition.name} already exists");
                break;
            }
        }

        if (transition == null)
            transition = lastState.AddTransition(firstState);

        transition.name = transitionName;
        transition.hasExitTime = true;
        transition.hasFixedDuration = true;
        transition.duration = 0.25f;
    }

    static string ExtractAnimationName(string path)
    {
        string animationName = Path.GetFileNameWithoutExtension(path).Split("@")[1];
        // stupid exception case
        if (animationName == "Idle4Legs")
            animationName = "idle4Legs";
        return animationName;
    }

    static string ExtractParentDirectory(string animationPath)
    {
        string parentDir = Path.Combine(animationPath.Split('/')[0..^2]).Replace("\\", "/");
        // Debug.Log($"AnimationName: {animationName} || ParentDir: {parentDir}");
        return parentDir;
    }

    static string MakeAnimControllerPath(string parentDir)
    {
        string animatorControllerGUID = AssetDatabase.FindAssets("*controller", new string[] { parentDir })[0];
        string animatorControllerPath = AssetDatabase.GUIDToAssetPath(animatorControllerGUID);
        // Debug.Log($"AnimController path: {animatorControllerPath}");
        return animatorControllerPath;
    }

    static AnimatorState FindAnimatorState(string animationName, AnimatorStateMachine rootStateMachine)
    {
        // Find the target state by name
        AnimatorState targetState = null;
        foreach (var state in rootStateMachine.states)
        {
            if (state.state.name == animationName)
                return state.state;
        }

        Debug.LogError($"State '{animationName}' not found in Animator Controller!");
        return targetState;
    }

    static void ChangeAllTransitions(AnimatorState targetState, bool keepTransition)
    {
        // Enable "Has Exit Time" or remove all transitions from the target state
        foreach (var transition in targetState.transitions)
        {
            if (keepTransition)
            {
                transition.hasExitTime = true;
            }
            else
            {
                targetState.RemoveTransition(transition);
            }
        }
    }
}
