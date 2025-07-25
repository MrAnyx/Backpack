using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Model;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Application.UseCase.Core.GetLoadingMessage;

public class GetLoadingMessageHandler : IQueryHandler<GetLoadingMessageQuery, string>
{
    private readonly string[] _englishMessages = [
        "Still faster than your morning coffee...",
        "Loading... bribing the hamsters to run faster.",
        "Hold on, aligning the stars...",
        "Downloading data from the cloud (hope it's not raining).",
    ];

    private readonly string[] _frenchMessages = [
        "Chargement… plus rapide que les embouteillages de Paris !",
        "On met les baguettes en place…",
        "Patientez... on cherche encore la connexion Wi-Fi du voisin.",
        "Presque prêt… le fromage affine encore un peu.",
    ];

    private readonly string[] _japaneseMessages = [
        "読み込み中… 忍者がデータをこっそり運んでいます。",
        "少々お待ちください…温泉から戻る途中です。",
        "データを召喚中…呪文がちょっと長いです。",
        "あとちょっと…猫が助けに来ています。",
    ];

    private readonly string[] _chineseMessages = [
        "加载中… 程序员在喝奶茶，请稍等。",
        "马上就好… 数据还在排队买早餐。",
        "加载中… 云端有点堵车。",
        "别急，程序正在翻墙回来。",
    ];

    private readonly string[] _koreanMessages = [
        "로딩 중… 치킨 먹는 중이라 조금만 기다려줘요!",
        "곧 시작합니다… 데이터가 지하철 타고 오는 중.",
        "잠시만요… 서버가 커피 타는 중입니다.",
        "로딩 중… 고양이가 도와주고 있어요.",
    ];

    private readonly string[] _spanishMessages = [
        "Cargando… aún más rápido que un lunes por la mañana.",
        "Un momento… el Wi-Fi está de siesta.",
        "Preparando todo… sin prisa, pero sin pausa.",
        "¡Casi listo! Los datos están calentando.",
    ];

    private readonly string[] _germanMessages = [
        "Lade... noch schneller als die Deutsche Bahn!",
        "Moment mal... wir sortieren noch die Datenwürste.",
        "Ladevorgang läuft… Kaffee wird erst fertig.",
        "Fast da! Der Server zieht noch seine Socken an.",
    ];

    private readonly string[] _russianMessages = [
        "Загрузка… медведь ещё не проснулся.",
        "Подождите… балалайка настраивается.",
        "Загружаем... почти как ракета, но с чайком.",
        "Секунду... матрёшки распаковываются."
    ];

    public Task<Result<string>> HandleAsync(GetLoadingMessageQuery query, RequestContext context, CancellationToken cancellationToken)
    {
        var messages = CultureInfo.CurrentCulture.TwoLetterISOLanguageName switch
        {
            "fr" => _frenchMessages,
            "en" => _englishMessages,
            "es" => _spanishMessages,
            "ja" => _japaneseMessages,
            "zh" => _chineseMessages,
            "ko" => _koreanMessages,
            "de" => _germanMessages,
            "ru" => _russianMessages,
            _ => _englishMessages,
        };
        var result = new Result<string>(messages[new Random().Next(0, messages.Length)]);
        return Task.FromResult(result);
    }
}
