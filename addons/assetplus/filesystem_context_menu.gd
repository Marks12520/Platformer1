@tool
extends EditorContextMenuPlugin

## Context menu plugin for FileSystem dock - adds AssetPlus options

const ExportDialog = preload("res://addons/assetplus/ui/export_dialog.gd")
const SettingsDialog = preload("res://addons/assetplus/ui/settings_dialog.gd")

var _selected_paths: PackedStringArray


func _popup_menu(paths: PackedStringArray) -> void:
	_selected_paths = paths

	if paths.is_empty():
		return

	# Check if selection contains at least one folder
	var has_folder = false
	for path in paths:
		if DirAccess.dir_exists_absolute(ProjectSettings.globalize_path(path)):
			has_folder = true
			break

	if not has_folder:
		return

	# Add "Export as .godotpackage" option
	var export_icon = EditorInterface.get_editor_theme().get_icon("Save", "EditorIcons")
	add_context_menu_item("Export as .godotpackage", _on_export_package, export_icon)


func _on_export_package(_callback_data: Variant = null) -> void:
	if _selected_paths.is_empty():
		return

	# Use first selected folder
	var folder_path = ""
	for path in _selected_paths:
		if DirAccess.dir_exists_absolute(ProjectSettings.globalize_path(path)):
			folder_path = path
			break

	if folder_path.is_empty():
		return

	# Check if global folder is configured
	var settings = SettingsDialog.get_settings()
	var global_folder = settings.get("global_asset_folder", "")

	if not global_folder.is_empty():
		# Ask if user wants to export to global folder
		var confirm = ConfirmationDialog.new()
		confirm.title = "Export to Global Folder?"
		confirm.dialog_text = "Do you want to export directly to your Global Folder?\n\n%s" % global_folder
		confirm.ok_button_text = "Yes, to Global Folder"
		confirm.cancel_button_text = "No, choose location"

		confirm.confirmed.connect(func():
			confirm.queue_free()
			_export_to_global_folder(folder_path, global_folder)
		)

		confirm.canceled.connect(func():
			confirm.queue_free()
			_export_normal(folder_path)
		)

		EditorInterface.get_base_control().add_child(confirm)
		confirm.popup_centered()
	else:
		# No global folder configured, just do normal export
		_export_normal(folder_path)


func _export_to_global_folder(folder_path: String, global_folder: String) -> void:
	var dialog = ExportDialog.new()
	EditorInterface.get_base_control().add_child(dialog)
	dialog.setup_for_global_folder(folder_path, global_folder)
	dialog.popup_centered()


func _export_normal(folder_path: String) -> void:
	var dialog = ExportDialog.new()
	EditorInterface.get_base_control().add_child(dialog)
	dialog.setup(folder_path)
	dialog.popup_centered()
