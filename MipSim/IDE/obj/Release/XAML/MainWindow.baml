   M S B A M L   `   `   `  ���� >  :IDE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:MipSim.IDE
MipSim.IDE      MipSim.IDE.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     ` \ICSharpCode.AvalonEdit, Version=4.3.2.9632, Culture=neutral, PublicKeyToken=9cc39be672370310?
avalonEdit.http://icsharpcode.net/sharpdevelop/avalonedit  )
validationclr-namespace:MipSim.IDE  R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  MIPS Simulator - Editor��
�������$
��1000��$	��600�� � 
ResizeMode$ CanMinimize=�.��]� .+�o� -   $��MenuMainWindow��$	��Top=�$��28��$��SemiBold"�����  �� EndPoint$	 0,1&� �� 
StartPoint$	 0,0&�.���  � Color$ 	#FFEAE8E8��� $ 	#FF00A3EF�� � Offset$ 0.944Q�� $ 	#FF057087��$ 1Q�.@�m� $��28��$��7,3,7,3q����� $
��File��$��Right=�$��Center=�.@�m� -   $	��New��m� -   $
��Open��m� -   $
��Save��m� -   $��28��$��7,3,7,3q����� $��Execute��$��Right=�$��Center=�m� $��28��$��7,3,7,3q����� $
��Help��$��Right=�$��Center=�.@�m� -   $
��Help��m� -   $��About��]�  J� IsManipulationEnabled . .+�� $��5q� � RowDefinitions �� $
��35px ��� $��* � � ColumnDefinitions �� $��* ��� $��5 ��� $	��120 ��� $	��180 ��� $��5 ��� $	��120 ��� $	��180 �.+��� $��0��$��0��$
��Code��$��Center=�$
��Bold"��� $��0��$��2��$��2��$��	Registers��$��Center=�$
��Bold"��� $��0��$��5��$��2��$��Memory��$��Center=�$
��Bold"��� -   $��taCode��$��1��$��0��$��3��$��1q�
���   �
���   �&   System.Windows.Controls.DataGrid  -	   $��RegGrid��$��1��$��2��$��2��	   AutoGenerateColumns	 .  
   CanUserReorderColumns
 .     CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .     ColumnHeaderHeight$ 30Q�   ClipboardCopyMode$ IncludeHeader=�   HeadersVisibility$ Column=�   VerticalGridLinesBrush ��  �� Color# � ��   HorizontalGridLinesBrush �� # � ��   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Register��   MinWidth$	 120Q�   Width5  /System.Windows.Controls.DataGridLengthConverter$	 120    
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding ��  �� Path$ Id�   ClipboardContentBinding Z�   $ Value (HEX)��$ *  Z�  �� $ HexValue� �� Mode$ TwoWay=� �� NotifyOnValidationError .  �� ValidatesOnExceptions .  �� FallbackValue$ 0��  �� ValidationRules     MipSim.IDE.HexValidation  !   EditingElementStyle! �� " �� 
TargetType"" ��.��� # �� Property# �@��	MaxLength$ �� Value$$ 16���� -
   $��
tbxGotoMem��$��0��$��6��$��0px 5px 0px 5pxq�% �� 	MaxLength$% 4��  -   $��MemGrid��$��1��$��5��$��2��	 .  
 .   .   .   .  $ 30Q�$ Column=� �� # � �� �� # � ��   $ Address��$	 120Q�$	 120  .  �� $ Address� Z�   $ Value (HEX)��$ *  Z�  �� $ HexValue�$ TwoWay=� .  . $ 0��    ! �� "" ��.��� # �@��	MaxLength$$ 2��