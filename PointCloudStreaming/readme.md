# PointClound Streaming from ROS

This Unity asset allows to render PointCloud2 from ROS into Unity. 

**NOTE: If your ros-sharp version is old, which uses `Subscrbier` instead of `UnitySubscriber`, you need to git clone [this particular commit](https://github.com/inmo-jang/unity_assets/commit/331e7e8eb78af0d583c730671b1a0ff2fe0a174f).**

### Dependency
- A rgb-d camera or Lidar ROS Package to receive PointCloud2 type data. This tutorial uses an [Orbbec Astra](https://orbbec3d.com/product-astra-pro/), for which you can use [ros_astra_camera](https://github.com/inmo-jang/ros_astra_camera_OLD). 
- [ros-sharp](https://github.com/siemens/ros-sharp): This connects a ROS-running PC to a Unity-running PC. To install this, see [here](https://github.com/siemens/ros-sharp/wiki/User_Inst_Unity3DOnWindows). 
- (Optional) [perception_pcl](https://github.com/inmo-jang/perception_pcl): This package has several filtering functions for pointclouds. This may be necessary to reduce the size of the pointcloud to send over to the Unity-running PC. Otherwise, the rendering speed in Unity may be slow. 

### Preparation in ROS side
An rgb-d camera or lidar should be running in a ROS-running PC, which is connected to a Unity-running PC by using a LAN cable. For example, you can do so as the following commands, which are actually for my personal purpose.  

1. Run a RGB-D camera: `roslaunch astra_camera astra.launch`
2. Downsampling the pointcloud: `roslaunch pcl_ros voxel_grid_filter.launch gui:=false`
3. Open a socket to Unity: `roslaunch rain_unity ur5_robotiq_unity_real.launch`

### Instruction in Unity side ([Video](https://youtu.be/yPPFK_74rro))
1. Copy this folder to "Assets" of your project.
2. Put `enableOpenGL.cs` on Main Camera object.
3. Put `PointCloudSubscriber.cs` on RosConnector object (Assuming that you already have ROSConnector). Then, set the name of pointcloud topic according to your ROS PC. 
3. Make a new game object, and put `PointCloudRenderer.cs`. 
4. Drag and drop the RosConnector object to `Subscriber`, and set `Point Size`. 
5. Make a new game object, which will be used to adjust the pointclouds' position, and place it into `Offset` in `PointCloudRenderer.cs`. 
6. Maybe you need to start Unity with `-force-opengl` option via command. 

### Further Details

#### File Explanation
1. enableOpenGL.cs
This script loads the openGL and uses glEnable to give us access to the `GL_VERTEX_PROGRAM_POINT_SIZE` which allows us to change the size of points rendered in the cloud.

2. PointCloudRenderer.cs
This script takes an array for the colour and and an array for the position of every point. 
Positions in Unity use a special class called Vector3 which define the x, y, z coords of a position. 
Colours in Unity are floating point values for RGB (and Alpha) between 0 and 1. 

The public float PointSize will allow you to change the size of all rendered points in the cloud, this can be changed
before and during runtime.

3. PointCloudShader.shader
This is a custom shader script which implements a float property called _PointSize. You don't need to do anything with this script, PLEASE DO NOT CHANGE ITS NAME. The pointCloud script automatically creates a material and assigns this custom shader to it
for rendering the point cloud.

4. Editor > GraphicsSettingsEditor.cs
This is an editor script which ensures the application is using OpenGL. If you attempt to change the graphics API in Project Settings > Player > Other Settings this script will automatically change the API back to OpenGL and warn you that the editor needs to be restarted.
You don't need to do anything with this script. The Point Size functionality depends on OpenGL being used, however, if you must use Direct3D or Vulkan then delete this script and good luck...

#### Understanding PointCloud2 data 

[PointCloud2](http://docs.ros.org/melodic/api/sensor_msgs/html/msg/PointCloud2.html) has data field where there are the information (e.g. position, intensity, rgb, etc.) of each point. It may look like [this](https://stackoverflow.com/questions/57507876/what-are-the-contents-of-pointcloud2). 

        header: 
          seq: 1071
          stamp: 
            secs: 1521699326
            nsecs: 676390000
          frame_id: "velodyne"
        height: 1
        width: 66811
        fields: 
          - 
            name: "x"
            offset: 0
            datatype: 7
            count: 1
          - 
            name: "y"
            offset: 4
            datatype: 7
            count: 1
          - 
            name: "z"
            offset: 8
            datatype: 7
            count: 1
          - 
            name: "intensity"
            offset: 16
            datatype: 7
            count: 1
          - 
            name: "ring"
            offset: 20
            datatype: 4
            count: 1
        is_bigendian: False
        point_step: 32
        row_step: 2137952
        data: [235, 171, 54, 190, 53, 107, 250, ...                

In this example, each point has 32 data field (as indicated by `point_step`), where its position is represented by `0`, `4`, and `8` th element (as indicated by `offset`). Likewise, the intensity is shown at `16`th element. This offset information is **IMPORTANT** to extract any necessary information from a Unity script. 

For your project, the data field may not look the same. So, I recommend you to investigate your data first by executing, for example, 

        rostopic echo -p /camera/depth/points > pcl.txt

Then, open the txt file, you will see how the data field look like. 
        
