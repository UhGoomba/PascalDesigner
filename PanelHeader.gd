extends PanelContainer

@onready var content : PanelContainer = get_node("../Content")
@onready var arrow : TextureRect = get_node("HBoxContainer/MarginContainer/Arrow")

func _on_gui_input(event):
	if event is InputEventMouseButton and event.pressed:
		if event.button_index == MOUSE_BUTTON_LEFT or event.button_index == MOUSE_BUTTON_MIDDLE:
			content.visible = !content.visible
			arrow.flip_v = !content.visible
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
