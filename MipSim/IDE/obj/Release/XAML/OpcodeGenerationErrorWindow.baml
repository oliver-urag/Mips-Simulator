   M S B A M L   `   `   `  ����   IDE,    &MipSim.IDE.OpcodeGenerationErrorWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     ` \ICSharpCode.AvalonEdit, Version=4.3.2.9632, Culture=neutral, PublicKeyToken=9cc39be672370310?
avalonEdit.http://icsharpcode.net/sharpdevelop/avalonedit  R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$/  )MIPS Simulator - Opcode Generation Errors��
�������$	��600��$	��400�� � 
ResizeMode$ CanMinimize=�.��]� .+�&   System.Windows.Controls.DataGrid  -   $��	ErrorGrid��$��10, 10 , 10, 10q�   AutoGenerateColumns .     CanUserReorderColumns .     CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .     ColumnHeaderHeight$ 30Q�   ClipboardCopyMode$ IncludeHeader=�	   HeadersVisibility$	 Column=�
   RowBackground
 �	#FFFBA8A8   
IsReadOnly .    VerticalGridLinesBrush ��  �� Color# � ��   HorizontalGridLinesBrush �� # � ��   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$
 Line��   MinWidth$ 50Q�   Width5  /System.Windows.Controls.DataGridLengthConverter$ 50    
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding ��  �� Path$ CodeLine�   ClipboardContentBinding Z�   $ Error��$ *  �� $ Message� Z�    ElementStyle �� .���  �� Property � �� �� Value$
 Wrap=�  $
 Text��$ *  �� $ 
CodeString� Z� 