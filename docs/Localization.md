# Localization

## Intro

localization is used for multiple languages support

## How to add a new language

1. translate specific resource files from zh <https://github.com/WeihanLi/ActivityReservation/tree/dev/ActivityReservation/Resources/zh>
2. add translated resource files into the `Resources` dir <https://github.com/WeihanLi/ActivityReservation/tree/dev/ActivityReservation/Resources>
3. add supported culture in `appsettings.json`, `Localization:SupportedCultures`

``` json
{
  "Localization": {
    "SupportedCultures": [ "zh", "en" ]
  }
}
```

## Contact

contact me if you need via email: <weihanli@outlook.com>
