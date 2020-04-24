#!/usr/bin/env python
# coding: utf-8

# In[ ]:


#!/usr/bin/env python
# coding: utf-8

# In[9]:

import sys
import pandas as pd
import numpy as np

#dataset = pd.read_csv("F:\Data set.csv")
sensorData = sys.argv[1]
dataset=pd.read_csv(sys.argv[2])

intermediate = sensorData.split(",")

for i in range(len(intermediate)-1):
    intermediate[i] = int(intermediate[i])

sensorDatafinal = intermediate[0:len(intermediate)-1]

 

Cell_Data = dataset.drop(columns = ['ACTION'])
Target_Action = dataset.ACTION


# In[11]:


from sklearn.neural_network import MLPClassifier
clf = MLPClassifier()
clf.fit(Cell_Data, Target_Action)


# In[12]:


dataset


# In[13]:


test_data = pd.DataFrame(sensorDatafinal).T.rename(columns={0:'P1',1:'P2',2:'P3',3:'P4',4:'P5',5:'P6',6:'P7',7:'P8',8:'P9'})


# In[14]:


# clf.predict(test_data)
#clf.predict(sensorData)

# In[15]:

# pd.DataFrame(clf.predict(test_data)).to_csv("Output.csv")

predict=clf.predict(test_data)
print(predict)

# In[ ]:


# In[ ]:


#test_data


# In[ ]:





# In[ ]:





# In[ ]:




