using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttackData)), CanEditMultipleObjects]
public class AttackDataEditor : Editor
{

    public SerializedProperty
        name_Prop,
        description_Prop,
        attackType_Prop,
        baseDamage_Prop,
        baseFireRate_Prop,
        isSingleFire_Prop,
        baseRecoilStrength_Prop,
        baseMaxMagazineSize_Prop,
        baseReloadTime_Prop,
        baseDuration_Prop,
        baseCooldown_Prop,
        hasPreAim_Prop,
        baseCastTime_Prop,
        baseCharges_Prop;

    private void OnEnable()
    {
        name_Prop                    = serializedObject.FindProperty("Name");
        description_Prop             = serializedObject.FindProperty("Description");
        attackType_Prop              = serializedObject.FindProperty("attackType");
        baseDamage_Prop              = serializedObject.FindProperty("baseDamage");
        baseFireRate_Prop            = serializedObject.FindProperty("baseFireRate");
        isSingleFire_Prop            = serializedObject.FindProperty("isSingleFire");
        baseRecoilStrength_Prop      = serializedObject.FindProperty("baseRecoilStrength");
        baseMaxMagazineSize_Prop     = serializedObject.FindProperty("baseMaxMagazineSize");
        baseReloadTime_Prop          = serializedObject.FindProperty("baseReloadTime");
        baseDuration_Prop            = serializedObject.FindProperty("baseDuration");
        baseCooldown_Prop            = serializedObject.FindProperty("baseCooldown");
        hasPreAim_Prop               = serializedObject.FindProperty("hasPreAim");
        baseCastTime_Prop            = serializedObject.FindProperty("baseCastTime");
        baseCharges_Prop             = serializedObject.FindProperty("baseChargeCount");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var attackData = (AttackData)target;

        EditorGUILayout.LabelField("Name:");
        name_Prop.stringValue = EditorGUILayout.TextField(attackData.Name, GUILayout.Width(250));

        EditorGUILayout.LabelField("Description:");
        GUIStyle areaStyle = new GUIStyle() { wordWrap = true };
        description_Prop.stringValue = EditorGUILayout.TextArea(attackData.Description, areaStyle, GUILayout.Height(50));

        EditorGUILayout.PropertyField(attackType_Prop);

        AttackDataType dt = (AttackDataType)attackType_Prop.enumValueIndex;

        switch (dt)
        {
            case AttackDataType.melee:
                baseDamage_Prop.intValue       = EditorGUILayout.IntField("Base Damage", attackData.baseDamage);
                EditorGUILayout.HelpBox("Set the fire rate by changing the samples in the animation.", MessageType.Info);
                isSingleFire_Prop.boolValue = EditorGUILayout.Toggle("Single Fire", attackData.isSingleFire);
                baseRecoilStrength_Prop.floatValue = EditorGUILayout.FloatField("Recoil Strength", attackData.baseRecoilStrength);
                baseMaxMagazineSize_Prop.intValue = EditorGUILayout.IntField("Max Magazine Size", attackData.baseMaxMagazineSize);
                baseReloadTime_Prop.floatValue = EditorGUILayout.FloatField("Reload Time", attackData.baseReloadTime);
                break;
            case AttackDataType.ranged:
                baseDamage_Prop.intValue       = EditorGUILayout.IntField("Base Damage", attackData.baseDamage);
                baseFireRate_Prop.floatValue   = EditorGUILayout.FloatField("Base Fire Rate", attackData.baseFireRate);
                isSingleFire_Prop.boolValue    = EditorGUILayout.Toggle("Single Fire", attackData.isSingleFire);
                baseRecoilStrength_Prop.floatValue = EditorGUILayout.FloatField("Recoil Strength", attackData.baseRecoilStrength);
                baseMaxMagazineSize_Prop.intValue = EditorGUILayout.IntField("Max Magazine Size", attackData.baseMaxMagazineSize);
                baseReloadTime_Prop.floatValue = EditorGUILayout.FloatField("Reload Time", attackData.baseReloadTime);
                break;
            case AttackDataType.passive:
                baseCooldown_Prop.floatValue       = EditorGUILayout.FloatField("Cooldown", attackData.baseCooldown);
                baseDuration_Prop.floatValue       = EditorGUILayout.FloatField("Duration", attackData.baseDuration);
                break;
            case AttackDataType.ultimate:
                baseDamage_Prop.intValue       = EditorGUILayout.IntField("Base Damage", attackData.baseDamage);
                baseCooldown_Prop.floatValue       = EditorGUILayout.FloatField("Cooldown", attackData.baseCooldown);
                baseDuration_Prop.floatValue       = EditorGUILayout.FloatField("Duration", attackData.baseDuration);
                hasPreAim_Prop.boolValue       = EditorGUILayout.Toggle("Uses aim indicator", attackData.hasPreAim);
                baseCastTime_Prop.floatValue       = EditorGUILayout.FloatField("Cast Time", attackData.baseCastTime);
                baseCharges_Prop.intValue          = EditorGUILayout.IntField("Charges", attackData.baseChargeCount);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

}