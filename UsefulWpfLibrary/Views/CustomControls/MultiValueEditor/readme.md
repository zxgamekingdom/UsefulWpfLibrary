# 定义

`MultiValueEditor`是一个根据其`DataContext`自动生成修改数据的WPF控件.

# 使用

1. 创建一个视图模型类
2. 创建视图模型类的对象
3. 将视图模型类的对象赋值给`MultiValueEditor`对象的`DataContext`属性

## 特性

### `IgnoreAttribute`

忽略指定的属性.该条属性不会生成对应的用于显示与修改的UI控件.

### `PropertyNameAttribute`

指定属性的显示名字.默认情况下`SetPropertyUserControl`控件的内容会生成N行2列的`Grid`,`Grid`每行的第一列显示的是视图模型属性的名字(设定其称呼为`Title`),`Title`默认情况下为属性的名字,可以通过该特性让`Title`更加的人性化.

### `OrderAttribute`

指定属性的显示顺序.默认情况下视图模型的属性是按照视图模型类属性定义的顺序来排序的.指定的显示顺序采用了先将未指定`OrderAttribute`的属性挑出,然后将指定了`OrderAttribute`的属性插入到未指定`OrderAttribute`的属性的列表中去.

### `CustomSingleValueEditorAttribute`

自定义用于显示属性的控件.默认情况下`bool`类型的属性会生成一个`CheckBox`,其余类型的属性都会生成一个`TextBox`.如果默认不能满足,则可以继承`CustomSingleValueEditorAttribute`,实现自己的显示方式.

### `TitleMenuItemAttribute`

在Title上添加`ContextMenu`.

### `TitleMenuItemUseCommandNameAttribute`

在Title上添加`ContextMenu`.

## `MultiValueEditorOptions`

`MultiValueEditor`会在生成控件时调用`MultiValueEditor`的`Options`属性或者`ViewModel`实现`IGetMultiValueEditorOptions`接口,该属性是`MultiValueEditorOptions`类型的.

优先使用实现了`IGetMultiValueEditorOptions`的`ViewModel`的Options.其次使用`MultiValueEditor`的`Options`属性.

该类委托执行的优先级是低于特性的.

