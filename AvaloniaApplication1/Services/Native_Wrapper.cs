using AvaloniaApplication1.Interfaces;
using AvaloniaApplication1.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
    
namespace AvaloniaApplication1.Services;
/*
public class NativeWrapper : INativeWrapper
{
    public Task<AnalyticsResult> CalculateAnalyticsAsync(RawData data)
    {
        // ЗАГЛУШКА - потом подключите реальную C++ библиотеку через P/Invoke
        var result = new AnalyticsResult(
            MostPopularService: data.Services
                .GroupBy(s => s.Name)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key ?? "Н/Д",
            TotalRevenue: data.Payments.Sum(p => p.Amount),
            BusiestDoctor: "Иванов И.И.", // временная заглушка
            AverageCheck: data.Payments.Any() ? data.Payments.Average(p => (double)p.Amount) : 0
        );
        
        return Task.FromResult(result);
    }

    public Task<string> EncryptDataAsync(string data)
    {
        // Вызов вашей libcrypto.so через DllImport
        return Task.FromResult(data); // заглушка
    }
}*/