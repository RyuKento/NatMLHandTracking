using UnityEngine;
using NatML.Devices;
using NatML.Devices.Outputs;
using NatML.Vision;
using NatML.Internal;
using NatML.Visualizers;

namespace NatML
{
    [MLEdgeModel.Embed("@natml/blazepalm-detector"), MLEdgeModel.Embed("@natml/blazepalm-landmark")]
    public class TestHandTracking : MonoBehaviour
    {
        /// <summary>
        /// Visualizer
        /// </summary>
        [Header(@"UI"), SerializeField]
        private BlazePalmVisualizer visualizer;

        /// <summary>
        /// Pipeline
        /// </summary>
        private BlazePalmPipeline pipeline;

        /// <summary>
        /// CameraDevice
        /// </summary>
        private CameraDevice cameraDevice;

        /// <summary>
        /// CameraDevice�Ŏʂ������̂�Texture�ɕϊ���������
        /// </summary>
        private TextureOutput previewTextureOutput;

        private void OnDisable() => pipeline?.Dispose();

        private async void Start()
        {
            // �J������������Ă��邩
            var permissionStatus = await MediaDeviceQuery.RequestPermissions<CameraDevice>();

            // ������Ă��Ȃ��ꍇ��return
            if (permissionStatus != PermissionStatus.Authorized) return;

            // �J�����擾
            var query = new MediaDeviceQuery(MediaDeviceCriteria.CameraDevice);
            cameraDevice = query.current as CameraDevice;

            // �J�����Ŏʂ������̂�Texture�ɕϊ�
            previewTextureOutput = new TextureOutput();
            cameraDevice.StartRunning(previewTextureOutput);
            var previewTexture = await previewTextureOutput;
            visualizer.image = previewTexture;

            // BlazePalm detector��predictor��NatML Hub����擾��Pipeline�̍쐬
            var detectorModelData = await MLEdgeModel.Create("@natml/blazepalm-detector");
            var predictorModelData = await MLEdgeModel.Create("@natml/blazepalm-landmark");
            //pipeline = new BlazePalmPipeline(detectorModelData, predictorModelData,0);
            pipeline = await BlazePalmPipeline.Create();
        }

        private void Update()
        {
            // Pipeline���쐬����Ă��邩
            if (pipeline == null) return;

            // �J�����Ŏʂ��Ă����񂩂���\��
            var hands = pipeline.Predict(previewTextureOutput.texture);

            // �\����������`��
            visualizer.Render(hands);
        }
    }
}