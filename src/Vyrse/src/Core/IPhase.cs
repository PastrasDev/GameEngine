using System;

namespace Vyrse.Core;

public interface IPhase
{
	void Load() { }
	void Initialize() { }
	void Start() { }
	void PreUpdate() { }
	void Update() { }
	void PostUpdate() { }
	void FixedUpdate() { }
	void Shutdown() { }

	public enum Phase
	{
		Load,
		Initialize,
		Start,
		PreUpdate,
		Update,
		PostUpdate,
		FixedUpdate,
		Shutdown
	}

	public static int Length => Names.Length;
	public static readonly Phase[] Values = Enum.GetValues<Phase>();
	public static readonly string[] Names = Array.ConvertAll(Values, phase => phase.ToString()!);
}