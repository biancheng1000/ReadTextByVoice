   M S B A M L   `   `   `  ����J  FReadTextByVoice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ReadTextByVoiceReadTextByVoice  5          ReadTextByVoice.MainView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:ReadTextByVoice     5        � 	Resources  5	       �� 
TargetType �� Property �� Value��%R   '��       �� 5
   
   " ��6   .�5      ��  � ��$ 60��6)   5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��135* �6   �� 5      $
��128* �6   �� 5      $
��119* �6   �� 5      $
��136* �6    � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $
��122* �6   �� 5      $	��29* �6   �� 5      $��50 �6   .+�5   
   �� $��Chapter��6   $��Center=�6#   $��Center=�6@   $��55,17,55,16.6q�6[   �� 5   
   $��1��63   $��5,5,4.8,4.6q�6C   ��6   �� *ChapterNameText+6C   �� 5   
   $
��Goto��6   $��2��6=   $��5.2,5,5.4,4.6q�6M   ��6    �� *
SerchCmd+6M   �� 5   
   $��Center=�6L   $��Center=�6i   $��3��6�   ��6   �� *(&SelectedBook.CurrentReadedChapter.Name+6�   �� 5    
    �� Orientation$ 
Horizontal=�6   $��1��6.   $��4��6;   $��0,0.4,0,0.2q�6O   .+�5!      �� $��选择��6   $��50��6"   $��5q�6O   ��6-   �� *SelectFileCmd+6O   �� 5"      $	��400��60    J� 	AllowDrop . $��40��6M   ��6   �� *
FilePath+6M   �� 5#      $��加载��6   $��50��6"   $��5q�6-   ��68   �� *LoadBookCmd+�� 5$      $��去除重复��6   $��50��6$   $��5q�6/   ��6:   �� *	RemoveCmd+5%      &   System.Windows.Controls.DataGrid  5'   
   $��2��6   $��4��6    $��5q�64      AutoGenerateColumns .  	   CanUserAddRows	 .  $	��141��5)   g   $	��Top=�6t   
 �� SelectedItem
 5(      �� *SelectedBook+��5)      �� *
AllBooks+   Columns 5*      0  *System.Windows.Controls.DataGridTextColumn  5+      ,  &System.Windows.Controls.DataGridColumn   MinWidth$	 100Q�6%      Header$ 书名��64   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6@   �� *
BookName+  5,      $	 100Q�6%   $ 	大小(M)��64    6C   �� *Size+  5-      $	 100Q�6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 64   $ 章节��6>    6J   �� *	Display+  5.      $	 100 6%   $ 进度��61    6=   �� *
Progress+50      �� 51   
    � Maximum$	 100Q�6   $��3��6   $��4��6,   $��5q�6@    � Minimum$	 0.1Q�6f    � Value 6K   �� *
Progress+6f   �� 52   
   $ 
Horizontal=�6   $��4��6/   $��94,4.6,94,5q�6<   $��4��6Q   $��Center=�6e   .+�53      �� $��«��6   $��28(�6!   ��6/   �� *
PreviouCmd+�� 54      $��||��6   $��20(�6"   $��5q�6N   ��60   �� *
PauseCmd+6N   �� 55      $	��▶��6   $��20(�6!   $��5q�6P   ��6/   �� *ReadTextCmd+6P   �� 56      $��»��6   $��28(�6!   $��5q�6L   ��6/   �� *	NextCmd+6L   �� 57      $��Reset��6   $��18(�6%   $��5q�6P   ��63   �� *	RestCmd+6P   �� 58      $
��Save��6   $��18(�6$   $��5q�6O   ��62   �� *	SaveCmd+6O   59      5:      5;      