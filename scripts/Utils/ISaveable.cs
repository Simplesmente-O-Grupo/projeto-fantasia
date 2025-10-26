
using Godot;

public interface ISaveable
{
	public Godot.Collections.Dictionary<string, Variant> GetSaveData();

	public bool LoadSaveData(Godot.Collections.Dictionary<string, Variant> saveData);
}