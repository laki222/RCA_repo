<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" name="Reddit" generation="1" functional="0" release="0" Id="fae5aba7-93e1-411e-940e-e155a303fda8" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="RedditGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="HealthMonitoringService:HealthMonitoring" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/Reddit/RedditGroup/LB:HealthMonitoringService:HealthMonitoring" />
          </inToChannel>
        </inPort>
        <inPort name="HealthStatusService:Endpoint1" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/Reddit/RedditGroup/LB:HealthStatusService:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="NotificationService:Notification" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/Reddit/RedditGroup/LB:NotificationService:Notification" />
          </inToChannel>
        </inPort>
        <inPort name="RedditService:Endpoint1" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/Reddit/RedditGroup/LB:RedditService:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="RedditService:RedditService" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/Reddit/RedditGroup/LB:RedditService:RedditService" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="HealthMonitoringService:DataConnectionStringLocal" defaultValue="">
          <maps>
            <mapMoniker name="/Reddit/RedditGroup/MapHealthMonitoringService:DataConnectionStringLocal" />
          </maps>
        </aCS>
        <aCS name="HealthMonitoringService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Reddit/RedditGroup/MapHealthMonitoringService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="HealthMonitoringServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Reddit/RedditGroup/MapHealthMonitoringServiceInstances" />
          </maps>
        </aCS>
        <aCS name="HealthStatusService:DataConnectionStringLocal" defaultValue="">
          <maps>
            <mapMoniker name="/Reddit/RedditGroup/MapHealthStatusService:DataConnectionStringLocal" />
          </maps>
        </aCS>
        <aCS name="HealthStatusService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Reddit/RedditGroup/MapHealthStatusService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="HealthStatusServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Reddit/RedditGroup/MapHealthStatusServiceInstances" />
          </maps>
        </aCS>
        <aCS name="NotificationService:DataConnectionStringLocal" defaultValue="">
          <maps>
            <mapMoniker name="/Reddit/RedditGroup/MapNotificationService:DataConnectionStringLocal" />
          </maps>
        </aCS>
        <aCS name="NotificationService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Reddit/RedditGroup/MapNotificationService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="NotificationServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Reddit/RedditGroup/MapNotificationServiceInstances" />
          </maps>
        </aCS>
        <aCS name="RedditService:DataConnectionStringLocal" defaultValue="">
          <maps>
            <mapMoniker name="/Reddit/RedditGroup/MapRedditService:DataConnectionStringLocal" />
          </maps>
        </aCS>
        <aCS name="RedditService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Reddit/RedditGroup/MapRedditService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="RedditServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Reddit/RedditGroup/MapRedditServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:HealthMonitoringService:HealthMonitoring">
          <toPorts>
            <inPortMoniker name="/Reddit/RedditGroup/HealthMonitoringService/HealthMonitoring" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:HealthStatusService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/Reddit/RedditGroup/HealthStatusService/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:NotificationService:Notification">
          <toPorts>
            <inPortMoniker name="/Reddit/RedditGroup/NotificationService/Notification" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:RedditService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/Reddit/RedditGroup/RedditService/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:RedditService:RedditService">
          <toPorts>
            <inPortMoniker name="/Reddit/RedditGroup/RedditService/RedditService" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapHealthMonitoringService:DataConnectionStringLocal" kind="Identity">
          <setting>
            <aCSMoniker name="/Reddit/RedditGroup/HealthMonitoringService/DataConnectionStringLocal" />
          </setting>
        </map>
        <map name="MapHealthMonitoringService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Reddit/RedditGroup/HealthMonitoringService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapHealthMonitoringServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Reddit/RedditGroup/HealthMonitoringServiceInstances" />
          </setting>
        </map>
        <map name="MapHealthStatusService:DataConnectionStringLocal" kind="Identity">
          <setting>
            <aCSMoniker name="/Reddit/RedditGroup/HealthStatusService/DataConnectionStringLocal" />
          </setting>
        </map>
        <map name="MapHealthStatusService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Reddit/RedditGroup/HealthStatusService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapHealthStatusServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Reddit/RedditGroup/HealthStatusServiceInstances" />
          </setting>
        </map>
        <map name="MapNotificationService:DataConnectionStringLocal" kind="Identity">
          <setting>
            <aCSMoniker name="/Reddit/RedditGroup/NotificationService/DataConnectionStringLocal" />
          </setting>
        </map>
        <map name="MapNotificationService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Reddit/RedditGroup/NotificationService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapNotificationServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Reddit/RedditGroup/NotificationServiceInstances" />
          </setting>
        </map>
        <map name="MapRedditService:DataConnectionStringLocal" kind="Identity">
          <setting>
            <aCSMoniker name="/Reddit/RedditGroup/RedditService/DataConnectionStringLocal" />
          </setting>
        </map>
        <map name="MapRedditService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Reddit/RedditGroup/RedditService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapRedditServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Reddit/RedditGroup/RedditServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="HealthMonitoringService" generation="1" functional="0" release="0" software="C:\Users\Lazar\Desktop\RCA_repo\Reddit\csx\Debug\roles\HealthMonitoringService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="HealthMonitoring" protocol="tcp" portRanges="10101" />
            </componentports>
            <settings>
              <aCS name="DataConnectionStringLocal" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;HealthMonitoringService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;HealthMonitoringService&quot;&gt;&lt;e name=&quot;HealthMonitoring&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;HealthStatusService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;NotificationService&quot;&gt;&lt;e name=&quot;Notification&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;RedditService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;RedditService&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Reddit/RedditGroup/HealthMonitoringServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Reddit/RedditGroup/HealthMonitoringServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Reddit/RedditGroup/HealthMonitoringServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="HealthStatusService" generation="1" functional="0" release="0" software="C:\Users\Lazar\Desktop\RCA_repo\Reddit\csx\Debug\roles\HealthStatusService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="https" portRanges="8080" />
            </componentports>
            <settings>
              <aCS name="DataConnectionStringLocal" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;HealthStatusService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;HealthMonitoringService&quot;&gt;&lt;e name=&quot;HealthMonitoring&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;HealthStatusService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;NotificationService&quot;&gt;&lt;e name=&quot;Notification&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;RedditService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;RedditService&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Reddit/RedditGroup/HealthStatusServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Reddit/RedditGroup/HealthStatusServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Reddit/RedditGroup/HealthStatusServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="NotificationService" generation="1" functional="0" release="0" software="C:\Users\Lazar\Desktop\RCA_repo\Reddit\csx\Debug\roles\NotificationService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Notification" protocol="tcp" portRanges="10100" />
            </componentports>
            <settings>
              <aCS name="DataConnectionStringLocal" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;NotificationService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;HealthMonitoringService&quot;&gt;&lt;e name=&quot;HealthMonitoring&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;HealthStatusService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;NotificationService&quot;&gt;&lt;e name=&quot;Notification&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;RedditService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;RedditService&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Reddit/RedditGroup/NotificationServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Reddit/RedditGroup/NotificationServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Reddit/RedditGroup/NotificationServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="RedditService" generation="1" functional="0" release="0" software="C:\Users\Lazar\Desktop\RCA_repo\Reddit\csx\Debug\roles\RedditService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="https" portRanges="80" />
              <inPort name="RedditService" protocol="tcp" portRanges="8081" />
            </componentports>
            <settings>
              <aCS name="DataConnectionStringLocal" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;RedditService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;HealthMonitoringService&quot;&gt;&lt;e name=&quot;HealthMonitoring&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;HealthStatusService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;NotificationService&quot;&gt;&lt;e name=&quot;Notification&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;RedditService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;RedditService&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Reddit/RedditGroup/RedditServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Reddit/RedditGroup/RedditServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Reddit/RedditGroup/RedditServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="RedditServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="HealthStatusServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="NotificationServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="HealthMonitoringServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="HealthMonitoringServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="HealthStatusServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="NotificationServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="RedditServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="HealthMonitoringServiceInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="HealthStatusServiceInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="NotificationServiceInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="RedditServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="cee17a3a-f5f8-41fc-a001-6eb02ddb86d2" ref="Microsoft.RedDog.Contract\ServiceContract\RedditContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="d7a89c3e-370c-4c34-9c67-6cde78bf23df" ref="Microsoft.RedDog.Contract\Interface\HealthMonitoringService:HealthMonitoring@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Reddit/RedditGroup/HealthMonitoringService:HealthMonitoring" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="ef5c9b6a-3967-48da-8c0e-2f007225328a" ref="Microsoft.RedDog.Contract\Interface\HealthStatusService:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Reddit/RedditGroup/HealthStatusService:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="1f9bc58f-9026-4cf2-b88a-6cff5790e843" ref="Microsoft.RedDog.Contract\Interface\NotificationService:Notification@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Reddit/RedditGroup/NotificationService:Notification" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="1512ccfd-f8a7-40f4-8ea3-14870747a9ba" ref="Microsoft.RedDog.Contract\Interface\RedditService:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Reddit/RedditGroup/RedditService:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="031eba9f-9cb8-47ce-a48d-314f9544c8bc" ref="Microsoft.RedDog.Contract\Interface\RedditService:RedditService@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Reddit/RedditGroup/RedditService:RedditService" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>