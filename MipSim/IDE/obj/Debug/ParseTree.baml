   M S B A M L   `   `   `  ����  IDE    IDE.ParseTree   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
Parse Tree��5   	   $	��300��6   $��558.209��6)    � 	Resources 5         NodeTemplate ��Name �� ItemsSource �� XPath �� Triggers �� Value �� Binding �� Path	 �� 
TargetName
 �� Value �� Property  xmlDataProvider��%�  &	        &	 �    �� 5      .R�5      �� $
 text��6   $��?��6*    5      �� 5	      $ child::node()��6    5      �� 5      $
 Text��6@    6   �� $ NodeType�.a�5      �� $
	 text��6   
 6?   �� $ Value� � ��6^   5      �� 5      $ Element��6@    6   �� $ NodeType�.a�5      �� $
	 text��6   
 6?   �� $
 Name� � ��6]   5      5      � 5      68   5      .��5   
   � $��20q�6   ��.  � ShowGridLines . $
��Auto=�6U    � RowDefinitions 5   
   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $	��350 �6   �� 5      $��* �6   .+�5    
   R� -   5        $��	treeView1��5"      $��0��5       $��0��5!      #��[  5$      ��5#      ��  �� Source# [ $ *��5$      �� 5%   
   -   5        $��
tblockTree��5%   /   $��0��6   $��1��6   $�� ��6A    �� 
IsReadOnly .  �� TextWrapping$ WrapWithOverflow=�6[   $��Consolas)�6{   5'      5(      