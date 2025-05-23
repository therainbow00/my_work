using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace auditTest
{
    public class Program
    {
        public void Main(string[] args)
        {
            /*
            $onPremFile = Import-Excel Get-ChildItem "$($env:USERPROFILE)\OneDrive - Nexus Pharma\Matt's scripts\powershell\AD reports\source\ADOnPremResult$(Get-Date -Format "MMM")-$(Get-Date -UFormat "%y").xlsx"
            $payFile = Import-Excel Get-ChildItem "$($env:USERPROFILE)\OneDrive - Nexus Pharma\Matt's scripts\powershell\AD reports\source\Paylocity $(Get-Date -Format "MMM") $(Get-Date -UFormat "%y").xlsx"
            $azureFile = Import-Excel Get-ChildItem "$($env:USERPROFILE)\OneDrive - Nexus Pharma\Matt's scripts\powershell\AD reports\source\AzureADResults$(Get-Date -Format "MMM")-$(Get-Date -UFormat "%y").xlsx"
            */
            string onPremFile = "";
            string payFile = "";
            string azureFile = "";
            /*
            [List[PSCustomObject]] $match = [List[PSCustomObject]]::new()
            [List[PSCustomObject]] $noPay = [List[PSCustomObject]]::new()
            [List[PSCustomObject]] $noAll = [List[PSCustomObject]]::new()
            [int] $i = 0
            [bool] $idMatch = $false
            [bool] $rejectMatch = $false
            [bool] $rejectAll = $true
            [int] $length = $onPremFile.Length
            $onPremFile | ForEach-Object {
                foreach ($entry in $_.psobject.properties)
                {
                    if ($entry.name -eq 'EmployeeID')
                    {
                        if (($entry.value -match '[a-z][A-Z]') -or ($entry.value -eq $null)) {
                            $rejectMatch = $true
                            [PSCustomObject] $reject = [PSCustomObject]::new()
                            [PSCustomObject] $rejectAllObj = [PSCustomObject]::new()
                            $rejectAllObj.psobject.properties.Add([psnoteproperty]::new($entry.name, $entry.value))
                            $reject.psobject.properties.Add([psnoteproperty]::new($entry.name, $entry.value))
                            continue
                        }
                        else
                        {
                            [PSCustomObject] $object = [PSCustomObject]::new()
                            [int] $userNumber = $entry.value
                            $payFile | ForEach-Object {
                                foreach ($entry1 in $_.psobject.properties)
                                {
                                    if ($entry1.name -eq 'Employee Id')
                                    {
                                        if ($userNumber -eq [int] $entry1.value)
                                        {
                                            $idMatch = $true
                                            [int] $entry1ValueNumber = [int] $entry1.value
                                            $object.psobject.properties.Add([psnoteproperty]::new($entry1.name, $entry1ValueNumber))
                                            continue
                                        }
                                        else {$idMatch = $false}
                                    }
                                    if ($idMatch)
                                    {
                                        $onPremFile | ForEach-Object {
                                            foreach ($property in $_.psobject.properties)
                                            {
                                                if ($property.name -eq 'EmployeeID')
                                                {
                                                    $property.value = $userNumber
                                                    if ($entry1.name -eq $property.name)
                                                    {
                                                        if ($entry1.value -eq $property.value)
                                                        {
                                                            Write-Host "$($entry1.name): $($entry1.value) matches $($property.name): $($property.value)"
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        $object.psobject.Properties.Add([psnoteproperty]::new($entry1.name, $entry1.value))
                                    }
                                    else {break}
                                }
                            }
                            $idMatch = $false
                            $azureFile | ForEach-Object {
                                foreach ($entry2 in $_.psobject.properties)
                                {
                                    if ($entry2.name -eq 'EmployeeId')
                                    {
                                        if ($entry2.value -notmatch '[a-z][A-Z]' -and $entry2.value -ne $null)
                                        {
                                            if ($userNumber -eq [int] $entry2.value)
                                            {
                                                $idMatch = $true
                                                continue
                                            }
                                        }
                                    }
                                    if ($idMatch) {$object.psobject.Properties.Add([psnoteproperty]::new($entry2.name, $entry2.value))}
                                    else { break }
                                }
                                $idMatch = $false
                            }
                            $match.Add($object)
                            break
                        }
                    }
                    else
                    {
                        if ($rejectMatch)
                        {
                            if ($entry.name -eq 'DisplayName')
                            {
                                $azureFile | ForEach-Object {
                                    foreach ($entry3 in $_.psobject.properties)
                                    {
                                        if ($entry3.name -eq 'DisplayName')
                                        {
                                            if ($entry3.value -eq $entry.value)
                                            {
                                                $rejectAll = $false
                                                $reject.psobject.properties.Add([psnoteproperty]::new($entry3.name, $entry3.value))
                                            }
                                            else {break}
                                        }
                                        elseif (!$rejectAll) {$reject.psobject.properties.Add([psnoteproperty]::new($entry3.name, $entry3.value))}
                                    }
                                    if (!$rejectAll)
                                    {
                                        $noPay.Add($reject)
                                        break
                                    }
                                }
                                if ($rejectAll) {$rejectAllObj.psobject.properties.Add([psnoteproperty]::new($entry.name, $entry.value))}
                            }
                            elseif ($rejectAll) {$rejectAllObj.psobject.properties.Add([psnoteproperty]::new($entry.name, $entry.value))}
                        }
                    }
                }
                if ($rejectMatch)
                {
                    $rejectMatch = $false
                    $rejectAll = $true
                    $noAll.Add($rejectAllObj)
                }
                Write-Progress -Activity 'Creating on-prem report...' -Status "$([math]::Round($($i / $length) * 100))%" -PercentComplete $([math]::Round($($i / $length) * 100)) -         CurrentOperation "$i / $length"
                $i++
            }
            
            #$match | Sort-Object 'Employee Id' | Export-Excel "$($env:USERPROFILE)\OneDrive - Nexus Pharma\Matt's scripts\powershell\AD reports\conversion\source\on-prem - pay and            azure.xlsx" -AutoSize
            #$noPay | Sort-Object 'DisplayName' | Export-Excel "$($env:USERPROFILE)\OneDrive - Nexus Pharma\Matt's scripts\powershell\AD reports\conversion\source\on-prem -            azure.xlsx" -AutoSize
            #$noAll | Sort-Object 'DisplayName' | Export-Excel "$($env:USERPROFILE)\OneDrive - Nexus Pharma\Matt's scripts\powershell\AD reports\conversion\source\on-prem - on-prem            only.xlsx" -AutoSize
                         */
        }
    }
}
