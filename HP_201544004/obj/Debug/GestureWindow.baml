   M S B A M L   `   `   `  ����  HP_2015440048    2Microsoft.Samples.Kinect.SkeletonBasics.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Skeleton Basics��5   	    � WindowState$ 	Maximized=�6O    � WindowStyle$
 None=�6g   $y�Visible=�6z    � AllowsTransparency . ��6�   Z�  � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  SquareCheckBox	 �� 
TargetType
 �� Orientation �Name ��Name ��Name �� TextAlignment �� Triggers P� Property P� Value �� 
TargetName��%1  &	        &	 #     &	 F     '�� i     &	 �     7  7  �� 5   
   $ 	#ff6e6e6e��62   �� 5   
   $ 	#ff52318f��64   �� 5	   
   $ 	#ff00BCF2��62   �� 5
   
   " ��6   .�5      ��  �@J�SnapsToDevicePixels . 64   5      �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      "	 ��6&   .R�5      � .+�5      �� $
 
Horizontal=�6)   
������ .+�5   "   � $ SquareCheckBoxChecked��6'   .+�5   &   �� $ CheckedNormal��6,   $��Images\CheckedNormal.png��6C   $
��None=�6e   $��Center=�6t   �� 5   &   $ CheckedHover��6,   $��Images\CheckedHover.png��6B   $
��None=�6c   $��Center=�6r   $y�	Collapsed=�6�   5   #   � 5   "   $ SquareCheckBoxUnchecked��6'   $y�	Collapsed=�6H   .+�5   &   �� $ UncheckedNormal��6,   $ ��Images\UncheckedNormal.png��6E   $
��None=�6i   $��Center=�6x   �� 5   &   $ UncheckedHover��6,   $��Images\UncheckedHover.png��6D   $
��None=�6g   $��Center=�6v   $y�	Collapsed=�6�   5   #   �� 5   "   $ SquareCheckBoxText��6,   #��z��6H   $
 Left=�6i   $��Center=�6   8��  $��15(�6�   $��9,0,0,0q�6�   5      5       5      P� 5       �@d�	IsChecked$ falseY�6;   . �5    "   �� $ SquareCheckBoxChecked��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5!   "   $ SquareCheckBoxUnchecked��6O    � y�$ Visible=�6?   6O   5"      P� 5#       �@J�IsMouseOver . . �5$   "   �� $ CheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5%   "   $ UncheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5&   "   $ CheckedHover��6O    � y�$ Visible=�6?   6O   �� 5'   "   $ UncheckedHover��6O    � y�$ Visible=�6?   6O   �� 5(   "   8  $ SquareCheckBoxText��6h    � ��5)      5+      5-      5.      5/      .��51      � -   5        $��
layoutGrid��51      $��0,0,2,1q�6    � RowDefinitions 52   
   �� 53      $
��Auto �6   �� 54      $��* �6   �� 55      $
��Auto �6   �� 56      $
��Auto �6   .+�58   
   (�  (� Stretch$ Uniform=�6   $��Center=�6$   $��2��6A   $��0,-1,0,1q�6R   $
��Auto��6d   .��59      �� -   5        $��Image��59      $
��1920��6!   $
��1320��6.   5:      �� 5;   
   -   5        $	��lbl��5;      $��현재 위치��6   $
��Left=�6+   $��	32,29,0,0q�6F   $��1��6Y   $	��Top=�6f   $	��100��6~   $	��374��6�   $��70(�6�   �� 5<   
   -   5        $��lbl_chk��5<      $��	제스쳐��6   $
��Left=�6-   $��
32,134,0,0q�6H   $��1��6\   $	��Top=�6i   $	��104��6�   $	��316��6�   $��70(�6�   �� 5=   
   -   5        $��	lbl_frame��5=      $��타임아웃��6"   $
��Left=�61   $��
32,369,0,0q�6L   $��1��6`   $	��Top=�6m   $	��106��6�   $	��355��6�   $��70(�6�   �� 5>   
   -   5        $��lbl_datachk��5>      $��싸이클횟수��6#   $
��Left=�63   $��
32,265,0,0q�6N   $��1��6b   $	��Top=�6o   $��99��6�   $	��332��6�   $��70(�6�   �� 5?   
   -   5        $��	lbl_mouse��5?      $��Label��6"   $
��Left=�62   $��
32,475,0,0q�6M   $��1��6a   $	��Top=�6n   $��49��6�   $	��332��6�   $��30(�6�   5@      5A      