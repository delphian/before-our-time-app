using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Services.Styles
{
    /// <summary>
    /// A single cohesive style for all controls
    /// </summary>
    public class StyleTemplate
    {
        /// <summary>
        /// Get specified style of picker from current template
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StylePicker GetPicker(StyleType type)
        {
            var picker = new StylePicker();
            if (type == StyleType.Primary)
            {
                picker.TextColor = "#00f000";
                picker.BackgroundColor = "#408040";
                picker.Margin = "10, 10, 10, 10";
                picker.FontSize = 14;
            }
            return picker;
        }
        /// <summary>
        /// Get picker style in xamarin form format
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Style GetPickerStyle(StyleType type)
        {
            var picker = GetPicker(type);
            var pickerStyle = new Style(typeof(Picker))
            {
                Setters =
                {
                    new Setter { Property = Picker.TextColorProperty, Value = Color.FromHex(picker.TextColor) },
                    new Setter { Property = Picker.BackgroundColorProperty, Value = Color.FromHex(picker.BackgroundColor) },
                    new Setter { Property = Picker.MarginProperty, Value = new Thickness(
                        Convert.ToDouble(picker.Margin.Split(',')[0].Trim()),
                        Convert.ToDouble(picker.Margin.Split(',')[1].Trim()),
                        Convert.ToDouble(picker.Margin.Split(',')[2].Trim()),
                        Convert.ToDouble(picker.Margin.Split(',')[3].Trim())) },
                    new Setter { Property = Picker.FontSizeProperty, Value = picker.FontSize }
                }
            };
            return pickerStyle;
        }
        /// <summary>
        /// Get specified style of button from current template
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StyleButton GetButton(StyleType type)
        {
            var button = new StyleButton();
            if (type == StyleType.Primary)
            {
                button.TextColor = "#00f000";
                button.BorderColor = "#60a060";
                button.BackgroundColor = "#408040";
                button.BorderRadius = 10;
                button.Margin = "10, 10, 10, 10";
                button.FontSize = 14;
            }
            if (type == StyleType.Warning)
            {
                button.TextColor = "#e0e000";
                button.BorderColor = "#d0d060";
                button.BackgroundColor = "#808040";
                button.BorderRadius = 2;
                button.Margin = "10, 10, 10, 10";
                button.FontSize = 14;
            }
            if (type == StyleType.Danger)
            {
                button.TextColor = "#f08080";
                button.BorderColor = "#d06060";
                button.BackgroundColor = "#804040";
                button.BorderRadius = 0;
                button.Margin = "10, 10, 10, 10";
                button.FontSize = 14;
            }
            return button;
        }
        /// <summary>
        /// Get button style in xamarin form format
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Style GetButtonStyle(StyleType type)
        {
            var button = GetButton(type);
            var buttonStyle = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter { Property = Button.TextColorProperty, Value = Color.FromHex(button.TextColor) },
                    new Setter { Property = Button.BorderColorProperty, Value = Color.FromHex(button.BorderColor) },
                    new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex(button.BackgroundColor) },
                    new Setter { Property = Button.CornerRadiusProperty, Value = button.BorderRadius },
                    new Setter { Property = Button.PaddingProperty, Value = 0 },
                    new Setter { Property = Button.MarginProperty, Value = new Thickness(
                        Convert.ToDouble(button.Margin.Split(',')[0].Trim()),
                        Convert.ToDouble(button.Margin.Split(',')[1].Trim()),
                        Convert.ToDouble(button.Margin.Split(',')[2].Trim()),
                        Convert.ToDouble(button.Margin.Split(',')[3].Trim())) },
                    new Setter { Property = Button.FontSizeProperty, Value = button.FontSize }
                }
            };
            return buttonStyle;
        }
        /// <summary>
        /// Get specified style editor from current template
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StyleEditor GetEditor(StyleType type)
        {
            var editor = new StyleEditor();
            if (type == StyleType.Primary)
            {
                editor.BackgroundColor = "#404040";
                editor.TextColor = "#ffffff";
                editor.FontSize = 14;
                editor.Margin = "10, 10, 10, 10";
            }
            return editor;
        }
        /// <summary>
        /// Get editor style in xamarin form format
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Style GetEditorStyle(StyleType type)
        {
            var editor = GetEditor(type);
            var editorStyle = new Style(typeof(Editor))
            {
                Setters =
                {
                    new Setter { Property = Editor.TextColorProperty, Value = Color.FromHex(editor.TextColor) },
                    new Setter { Property = Editor.BackgroundColorProperty, Value = Color.FromHex(editor.BackgroundColor) },
                    new Setter { Property = Editor.FontSizeProperty, Value = editor.FontSize },
                    new Setter { Property = Button.MarginProperty, Value = new Thickness(
                        Convert.ToDouble(editor.Margin.Split(',')[0].Trim()),
                        Convert.ToDouble(editor.Margin.Split(',')[1].Trim()),
                        Convert.ToDouble(editor.Margin.Split(',')[2].Trim()),
                        Convert.ToDouble(editor.Margin.Split(',')[3].Trim())) }
                 }
            };
            return editorStyle;
        }
        /// <summary>
        /// Get specified style entry from current template
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StyleEntry GetEntry(StyleType type)
        {
            var entry = new StyleEntry();
            if (type == StyleType.Primary)
            {
                entry.BackgroundColor = "#404040";
                entry.TextColor = "#ffffff";
                entry.PlaceholderColor = "#ffffff";
                entry.Margin = "10, 2, 10, 2";
            }
            return entry;
        }
        /// <summary>
        /// Get entry style in xamarin form format
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Style GetEntryStyle(StyleType type)
        {
            var entry = GetEntry(type);
            var entryStyle = new Style(typeof(Entry))
            {
                Setters =
                {
                    new Setter { Property = Editor.TextColorProperty, Value = Color.FromHex(entry.TextColor) },
                    new Setter { Property = Editor.BackgroundColorProperty, Value = Color.FromHex(entry.BackgroundColor) },
                    new Setter { Property = Editor.PlaceholderColorProperty, Value = Color.FromHex(entry.PlaceholderColor) },
                    new Setter { Property = Button.MarginProperty, Value = new Thickness(
                        Convert.ToDouble(entry.Margin.Split(',')[0].Trim()),
                        Convert.ToDouble(entry.Margin.Split(',')[1].Trim()),
                        Convert.ToDouble(entry.Margin.Split(',')[2].Trim()),
                        Convert.ToDouble(entry.Margin.Split(',')[3].Trim())) }
                }
            };
            return entryStyle;
        }
        /// <summary>
        /// Get specified style page from current template
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StylePage GetPage(StyleType type)
        {
            var page = new StylePage();
            if (type == StyleType.Primary)
            {
                page.BackgroundColor = "#202020";
            }
            return page;
        }
    }
}