# PointClound Streaming from ROS

This asset allows to render PointCloud2 from ROS into Unity

- Made by Dr Inmo Jang (inmo.jang@manchester.ac.uk / inmo3592@gmail.com)

### Instruction 
1. ....

### Further Details
#### Understanding PointCloud2 data 

[PointCloud2](http://docs.ros.org/melodic/api/sensor_msgs/html/msg/PointCloud2.html) has data field where there are the information (e.g. position, intensity, rgb, etc.) of each point. It may look like [this](https://answers.ros.org/question/58112/how-can-i-save-a-msg-to-a-file/). 

In this example, each point has 32 data field (as shown in `point_step`), where its position is represented by `0`, `4`, and `8` th element (as shown in `offset`). Likewise, the intensity is shown from `16`th element. This offset information is IMPORTANT to extract any necessary information from a Unity script. 

For your project, the data field may not look the same. So, I recommend you to investigate your data first by executing, for example, 

        rostopic echo -p /camera/depth/points > pcl.txt

Then, open the txt file, you will see how the data field look like. 
        
