using Godot;
using System;

public partial class FadeAnimation : ColorRect
{
    private AnimationPlayer animationPlayer;
    
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    
    public void PlayFadeInAnimation()
    {
        animationPlayer.Play("FadeIn");
    }
}
