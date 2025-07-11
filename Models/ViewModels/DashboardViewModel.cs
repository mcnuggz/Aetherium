﻿namespace Aetherium.Models.ViewModels
{
    public class DashboardViewModel
    {
        public CharacterModel CurrentCharacter { get; set; }
        public List<CharacterModel> AllCharacters { get; set; }
        public List<PostViewModel> Posts { get; set; }
    }
}
