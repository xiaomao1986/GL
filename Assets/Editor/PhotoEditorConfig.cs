using UnityEngine;
using UnityEditor;
using System.Collections;


  [CustomEditor(typeof(VariableConfig))]
public class PhotoEditorConfig : Editor
{



      private VariableConfig m_VariableConfig;


      public override void OnInspectorGUI()
      {
          m_VariableConfig = (VariableConfig)target;

          m_VariableConfig.ClickName = EditorGUILayout.TextField("--------------------------单张功能配置-----------------------");
          m_VariableConfig.Click_Move_pos = EditorGUILayout.Vector3Field("相片飞到的位置：", m_VariableConfig.Click_Move_pos);
          m_VariableConfig.Click_Move_Speed = EditorGUILayout.FloatField("相片飞到的位置速度：", m_VariableConfig.Click_Move_Speed);
          m_VariableConfig.Click_ui_color1 = EditorGUILayout.ColorField("过度颜色1：", m_VariableConfig.Click_ui_color1);
          m_VariableConfig.Click_ui_color2 = EditorGUILayout.ColorField("过度颜色2：", m_VariableConfig.Click_ui_color2);
          m_VariableConfig.Click_ui_Speed = EditorGUILayout.FloatField("透明变化速度：", m_VariableConfig.Click_ui_Speed);
          m_VariableConfig.Click_ui_Speed2 = EditorGUILayout.FloatField("缩小放大变化速度：", m_VariableConfig.Click_ui_Speed2);

          m_VariableConfig.FlyManName = EditorGUILayout.TextField("--------------------------收集功能配置-----------------------");
          m_VariableConfig.FlyMan_Grad_pos = EditorGUILayout.Vector3Field("相片飞到的位置：", m_VariableConfig.FlyMan_Grad_pos);
          m_VariableConfig.FlyMan_photo_Scale = EditorGUILayout.Vector3Field("相片飞入大小：", m_VariableConfig.FlyMan_photo_Scale);
          m_VariableConfig.FlyMan_photo_speed = EditorGUILayout.FloatField("相片飞入速度：", m_VariableConfig.FlyMan_photo_speed);
          m_VariableConfig.FlyMan_ui_color1 = EditorGUILayout.ColorField("过度颜色1：", m_VariableConfig.FlyMan_ui_color1);
          m_VariableConfig.FlyMan_ui_color2 = EditorGUILayout.ColorField("过度颜色2：", m_VariableConfig.FlyMan_ui_color2);
          m_VariableConfig.FlyMan_ui_Speed = EditorGUILayout.FloatField("透明变化速度：", m_VariableConfig.FlyMan_ui_Speed);
          m_VariableConfig.FlyMan_ui_Speed2 = EditorGUILayout.FloatField("缩小放大变化速度：", m_VariableConfig.FlyMan_ui_Speed2);

          m_VariableConfig.ScaleName = EditorGUILayout.TextField("--------------------------缩放功能配置-----------------------");
          m_VariableConfig.Photo_Scale_adj = EditorGUILayout.FloatField("缩小距离限制：", m_VariableConfig.Photo_Scale_adj);
          m_VariableConfig.Photo_Scale_big = EditorGUILayout.FloatField("放大距离限制：", m_VariableConfig.Photo_Scale_big);
          m_VariableConfig.Photo_Scale_Speed = EditorGUILayout.FloatField("缩放速度：", m_VariableConfig.Photo_Scale_Speed);

          m_VariableConfig.photoEnterName = EditorGUILayout.TextField("--------------------------进场功能配置-----------------------");
          m_VariableConfig.Photo_Enter_high = EditorGUILayout.FloatField("出场高度：", m_VariableConfig.Photo_Enter_high);
          m_VariableConfig.Photo_Enter_Speed = EditorGUILayout.FloatField("下落速度：", m_VariableConfig.Photo_Enter_Speed);
          m_VariableConfig.Photo_Load_Speed = EditorGUILayout.FloatField("下载框边旋转速度：", m_VariableConfig.Photo_Load_Speed);
          m_VariableConfig.Photo_Flash_Speed = EditorGUILayout.FloatField("流光速度：", m_VariableConfig.Photo_Flash_Speed);
          m_VariableConfig.Photo_pot_Speed = EditorGUILayout.FloatField("旋转速度：", m_VariableConfig.Photo_pot_Speed);

          m_VariableConfig.RespondName = EditorGUILayout.TextField("--------------------------呼应功能配置-----------------------");
          m_VariableConfig.Photo_Respond_Speed = EditorGUILayout.FloatField("呼应速度：", m_VariableConfig.Photo_Respond_Speed);
          m_VariableConfig.Photo_Respond_angle = EditorGUILayout.FloatField("角度：", m_VariableConfig.Photo_Respond_angle);
          if (GUI.changed)

          {
              EditorUtility.SetDirty(target);
          }

      }

}
