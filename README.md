# Planets
WPF App for planets and their properties administration

#DB
SqLite and EF Core Code first used to store data. DB filess in root dir.
Wome data for testing in DB, join table PLanetProporty used to store property values for different planets.

#Filtering
Planets can be filtered by Properties, if TextBox is empty then Planets which have selected Property in combo are displayed.
If Textbox has value then all Planets which have Property that contains value from Textbox are filtered, not case sensitive.

#Planets Listbox
If any Planet is selected then Planet name can be updated or deleted if it doesn't contain any Properties.

#Planet Properties DataGrid
If any Planet is selected then all Properies and there values assigned to this planet are displayed in DataGrid (DetailWiev.xml)
If no Property is selected in DataGrid then only Properties that are defined and not assigned value in Datagrid can be added.
If any Property is selected in Datagrid then only Value can be changed to add new Property, the Property must be deselected by changing Planet.

#Defining new Properties
Available Properties can modified in listbox (PropertyView.xaml) at the bottom.
