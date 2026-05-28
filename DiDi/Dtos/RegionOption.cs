namespace DiDi.Dtos;

/// <summary>
/// 規格書中的地區可選項。
/// </summary>
/// <param name="Province">省份。</param>
/// <param name="Cities">該省份底下可選擇的城市。</param>
public sealed record RegionOption(string Province, IReadOnlyList<string> Cities);
